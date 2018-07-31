using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static LiveChatRoom.App_Classes.CustomValidators;

namespace LiveChatRoom.Models
{
    public class ChangeAvatarModel
    {
        [Required]
        [Display(Name = "Avatar")]
        [IsImageUrl(ErrorMessage = "It isn't image url!")]
        public string Avatar { get; set; }
    }
}