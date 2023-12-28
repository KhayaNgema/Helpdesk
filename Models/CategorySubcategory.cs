using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class CategorySubcategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategorySubCategoryId { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Categories { get; set; }

        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategories { get; set; }
    }
}