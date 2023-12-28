using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        private string _productKey;

        public string ProductKey
        {
            get => _productKey;
            set => _productKey = ProcessProductKey(value);
        }

        public string ProductDescription { get; set; }

        // Method to process the ProductKey
        private string ProcessProductKey(string inputKey)
        {
            // Convert to uppercase
            string processedKey = inputKey.ToUpper();

            // If the length is less than 4, pad with underscores
            if (processedKey.Length < 4)
            {
                processedKey = processedKey.PadRight(4, '_');
            }
            // If the length is more than 4, truncate to 4 characters
            else if (processedKey.Length > 4)
            {
                processedKey = processedKey.Substring(0, 4);
            }

            return processedKey;
        }


        [JsonIgnore]
        public virtual ICollection<ClientProduct> ClientProducts { get; set; }
    }
}