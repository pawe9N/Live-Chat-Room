using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LiveChatRoom.Models;
using System.Threading.Tasks;

namespace LiveChatRoom.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<string, DateTime> activeUsers = new Dictionary<string, DateTime>();

        public void Send(string name, string message, string gender)
        {
            DateTime date = DateTime.UtcNow;
            if (!activeUsers.ContainsKey(name))
            {
                activeUsers.Add(name, date);
            }
            else
            {
                activeUsers[name] = date;
            }
            Clients.All.addNewMessageToPage(name, message, gender);
        }

        //if last message from user was 3 minutes ago then he will be removed from active users
        private void CheckActiveConnections()
        {
            foreach (KeyValuePair<string, DateTime> entry in activeUsers)
            {
                if(entry.Value.AddMinutes(3) <= DateTime.UtcNow)
                {
                    activeUsers.Remove(entry.Key);
                }
            }
        }

        //return list of all active users
        public List<string> GetAllActiveConnections()
        {
            CheckActiveConnections();
            List<string> userNames = new List<string>();
            foreach (KeyValuePair<string, DateTime> entry in activeUsers)
            {
                userNames.Add(entry.Key); 
            }
            return userNames;
        }
    }
}