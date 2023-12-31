using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helpdesk.Models
{
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        public string NotificationSubject { get; set; }

        // Sender of the notification
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        // Recipient of the notification
        public string RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual ApplicationUser Recipient { get; set; }

        // Add RoleId property
        [NotMapped]
        public string RoleId { get; set; }

        // Add navigation property for Role
        [NotMapped]
        public virtual IdentityUserRole Role { get; set; }

        public DateTime NotificationDate { get; set; }

        public bool IsRead { get; set; }

        public bool IsNew { get; set; }

        public string FullSenderName => $"{Sender.FirstName} {Sender.LastName}";
    }
}
