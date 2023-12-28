using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class ClientProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientProductId { get; set; }

        public int OnboardingId { get; set; }
        public virtual ApprovedRequest ApprovedRequest { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}