using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class ClientOnboarding : PrimaryContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Request Id")]
        public int OnboardingId { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Client Address")]
        public string OfficeAddress { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }    

        [Display(Name = "Client Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Period From")]
        public DateTime PeriodFrom { get; set; }

        [Display(Name = "Period To")]
        public DateTime PeriodTo { get; set; }

        [Display(Name = "Operation Hours(Open)")]
        [DataType(DataType.Time)]
        public TimeSpan OpretaionalHoursOpen { get; set; }

        [Display(Name = "Operating Hours(Close)")]
        [DataType(DataType.Time)]
        public TimeSpan OpretationalHourseClose { get; set; }

        [Display(Name = "Request Status")]
        public RequestStatus Status { get; set; }



        [Display(Name = "Client Name Abbreviation")]
        public string ClientAbbr
        {
            get => GenerateClientAbbreviation(ClientName);
            set { /* You can add a setter if necessary */ }
        }

        // Method to generate client abbreviation
        private string GenerateClientAbbreviation(string clientName)
        {
            if (string.IsNullOrWhiteSpace(clientName))
            {
                return string.Empty;
            }

            // Extract first letters of each word and convert to uppercase
            var abbreviation = new string(clientName.Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => word[0])
                .ToArray())
                .ToUpper(CultureInfo.InvariantCulture);

            return abbreviation;
        }
    }

    public class DeclinedRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Request Id")]
        public int OnboardingId { get; set; }

        public PersonalTitle Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }


        public string PhoneNumber { get; set; }
        public int DesignationId { get; set; }

        public string EmployeeOfficeAddress { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Client Address")]
        public string OfficeAddress { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [Display(Name = "Client Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Period From")]
        public DateTime PeriodFrom { get; set; }

        [Display(Name = "Period To")]
        public DateTime PeriodTo { get; set; }

        [Display(Name = "Operation Hours(Open)")]
        [DataType(DataType.Time)]
        public TimeSpan OpretaionalHoursOpen { get; set; }

        [Display(Name = "Operating Hours(Close)")]
        [DataType(DataType.Time)]
        public TimeSpan OpretationalHourseClose { get; set; }

        [Display(Name = "Request Status")]
        public RequestStatus Status { get; set; }



        [Display(Name = "Client Name Abbreviation")]
        public string ClientAbbr
        {
            get => GenerateClientAbbreviation(ClientName);
            set { /* You can add a setter if necessary */ }
        }

        // Method to generate client abbreviation
        private string GenerateClientAbbreviation(string clientName)
        {
            if (string.IsNullOrWhiteSpace(clientName))
            {
                return string.Empty;
            }

            // Extract first letters of each word and convert to uppercase
            var abbreviation = new string(clientName.Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => word[0])
                .ToArray())
                .ToUpper(CultureInfo.InvariantCulture);

            return abbreviation;
        }
    }

    public class ApprovedRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Request Id")]
        public int OnboardingId { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        public PersonalTitle Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public int DesignationId { get; set; }

        public string EmployeeOfficeAddress { get; set; }

        [Display(Name = "Client Address")]
        public string OfficeAddress { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }    

        [Display(Name = "Client Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Period From")]
        public DateTime PeriodFrom { get; set; }

        [Display(Name = "Period To")]
        public DateTime PeriodTo { get; set; }

        [Display(Name = "Operation Hours(Open)")]
        [DataType(DataType.Time)]
        public TimeSpan OpretaionalHoursOpen { get; set; }

        [Display(Name = "Operating Hours(Close)")]
        [DataType(DataType.Time)]
        public TimeSpan OpretationalHourseClose { get; set; }

        [Display(Name = "Request Status")]
        public RequestStatus Status { get; set; }



        [Display(Name = "Client Name Abbreviation")]
        public string ClientAbbr
        {
            get => GenerateClientAbbreviation(ClientName);
            set { /* You can add a setter if necessary */ }
        }

        // Method to generate client abbreviation
        private string GenerateClientAbbreviation(string clientName)
        {
            if (string.IsNullOrWhiteSpace(clientName))
            {
                return string.Empty;
            }

            // Extract first letters of each word and convert to uppercase
            var abbreviation = new string(clientName.Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => word[0])
                .ToArray())
                .ToUpper(CultureInfo.InvariantCulture);

            return abbreviation;
        }
        // Break the circular reference by ignoring this navigation property during serialization
        [JsonIgnore]
        public virtual ICollection<ClientProduct> ClientProducts { get; set; }

    }



    public class PrimaryContact
    {
        [Display(Name = "Title")]

        public PersonalTitle Title { get; set; }

        [Display(Name = "First Name(s)")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public virtual Designation Designations { get; set; }

        [Display(Name = "Employee Office Address")]
        public string EmployeeOfficeAddress { get; set; }
    }

    public class Country
    {
        [Key]  // This is the key attribute to specify the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string Name { get; set; }

        [Display(Name = "Alpha3 Code")]
        public string Alpha3Code { get; set; }
    }





    public enum RequestStatus
    {
        Pending,
        Approved,
        Declined

    }

    public enum PersonalTitle
    {
        Mr,
        Mrs,
        Miss,
        Ms,
        Dr,
        Prof,
        Rev,
        Capt,
        Sir,
        Lady,
        Lord,
    }


    public class Designation
    {
        [Display(Name = "Designation")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }

        [Display(Name = "Designation")]
        public string DesignationName { get; set; }
    }
}