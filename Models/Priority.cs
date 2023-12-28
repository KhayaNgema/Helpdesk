using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class Priority
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriorityId { get; set; }

        public int PriorityNo { get; set; }
        public string PriorityName { get; set; }
    }

    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }


    public class SubCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }
        public string faultCode { get; set; }
        public string SubCategoryName { get; set; }

        public int PriorityId { get; set; } // e.g., High, Medium, Low
        public virtual Priority Priority { get; set; }
        public int SLAValueId { get; set; }
        public virtual SLAValue SLAValue { get; set; }

    }

    public class SLAValue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SLAValueId { get; set; }

        public string SLAValueName { get; set; }

    }
}