using System.Linq;
using Helpdesk.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Helpdesk.Services
{
    public class CountryApiHelper
    {
        private const string ApiBaseUrl = "https://restcountries.com/v3.1/";

        private readonly ApplicationDbContext _context;

        public CountryApiHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveCountriesToDatabase()
        {
            var countries = GetCountries();

            if (countries != null && countries.Any())
            {
                // Save or update countries in the database
                foreach (var country in countries)
                {
                    var existingCountry = _context.Countries.SingleOrDefault(c => c.CountryId == country.CountryId);

                    if (existingCountry == null)
                    {
                        // Add a new country if it doesn't exist in the database
                        _context.Countries.Add(country);
                    }
                    else
                    {
                        // Update existing country if needed
                        existingCountry.Name = country.Name;
                        // Update other properties as needed
                    }
                }

                _context.SaveChanges();
            }
        }

        public List<Country> GetCountries()
        {
            var client = new RestClient(ApiBaseUrl);
            var request = new RestRequest("all", Method.GET);

            var response = client.Execute<List<Dictionary<string, object>>>(request);

            if (response.IsSuccessful)
            {
                var countries = response.Data
                    .Select(countryData => new Country
                    {
                        Alpha3Code = countryData.ContainsKey("alpha3Code") ? countryData["alpha3Code"]?.ToString() : null,
                        Name = countryData.ContainsKey("name") && countryData["name"] is Dictionary<string, object> nameDict && nameDict.ContainsKey("common")
                            ? nameDict["common"]?.ToString()
                            : null
                    })
                    .OrderBy(country => country.Name)
                    .ToList();

                return countries;
            }

            return null;
        }
    }
}
