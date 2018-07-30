using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace LiveChatRoom.App_Classes
{
    //Class for sender email data
    public class SenderEmail
    {
        public string email; //Replace with actual email
        public string password;   //Replace with actual password

        //Get senderEmail data from file
        public SenderEmail()
        {
            using (StreamReader r = new StreamReader(HostingEnvironment.MapPath("~/Content/senderEmailData.json")))
            {
                string json = r.ReadToEnd();
                var senderData = JObject.Parse(json);
                email = senderData["email"].ToString();
                password = senderData["password"].ToString();
            }
        }
    }
}