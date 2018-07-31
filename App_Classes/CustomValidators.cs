using LiveChatRoom.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace LiveChatRoom.App_Classes
{
    public static class CustomValidators
    {
        //to check if user is adult
        public class DateOfBirthAdultAttribute : ValidationAttribute
        {
            public static bool IsUserAdult(DateTime? date)
            {
                DateTime maxDate = DateTime.Now.AddYears(-18);
                DateTime minDate = DateTime.Now.AddYears(-100);

                return (date >= minDate && date <= maxDate);
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime? date = (DateTime?)value;
                if (!IsUserAdult(date))
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
                return null;
            }
        }

        //to check if email is exist in database
        public class IsEmailExistAttribute : ValidationAttribute
        {
            private bool IsEmailExist(string emailID)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                    return v != null;
                }
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string emailID = (string)value;
                if (IsEmailExist(emailID))
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
                return null;
            }
        }

        //check if uri is valid
        public class IsImageUrlAttribute : ValidationAttribute
        {
            public static bool IsImageUrl(string URL)
            {
                if (!string.IsNullOrEmpty(URL))
                {
                    bool result = Uri.TryCreate(URL, UriKind.Absolute, out Uri uri)
                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

                    if (result)
                    {
                        var req = (HttpWebRequest)HttpWebRequest.Create(URL);
                        req.Method = "HEAD";
                        using (var resp = req.GetResponse())
                        {
                            if( resp.ContentType.ToLower(CultureInfo.InvariantCulture).StartsWith("image/"))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string url = (string)value;
                if (!IsImageUrl(url))
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
                return null;
            }
        }
    }
}