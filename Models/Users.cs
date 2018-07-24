using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiveChatRoom.Models
{
    public static class Users
    {
        static public int usersCount = 1;
        static public Dictionary<string, int> usersDictionary = new Dictionary<string, int>();
    }
}