using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LiveChatRoom.Models;

namespace LiveChatRoom.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            if (!Users.usersDictionary.ContainsKey(name))
            {
                Users.usersDictionary[name] = Users.usersCount++;
            }

            Clients.All.addNewMessageToPage(name, message, Users.usersDictionary[name]);
        }
    }
}