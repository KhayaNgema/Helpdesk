using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Services
{
    public class NotificationHub : Hub
    {
        public void SendNotification(string userId, string message)
        {

            Console.WriteLine($"Sending notification to {userId}: {message}");
            Clients.User(userId).receiveNotification(message);
        }
    }

}