using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class DailySequentialNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNumberId { get; set; }

        public DateTime Date { get; set; }

        public int SequentialNumber { get; set; }
    }
}