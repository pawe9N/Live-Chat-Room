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
            if (!UsersInChat.usersDictionary.ContainsKey(name))
            {
                UsersInChat.usersDictionary[name] = UsersInChat.usersCount++;
            }

            Clients.All.addNewMessageToPage(name, message, UsersInChat.usersDictionary[name]);
        }
    }
}