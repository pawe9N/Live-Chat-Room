using System;
using System.ComponentModel.DataAnnotations;
using static LiveChatRoom.App_Classes.CustomValidators;

namespace LiveChatRoom.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User name is required")]
        [MaxLength(24, ErrorMessage = "Maximum 24 characters required")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters required")]
        public string UserName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID is required")]
        [DataType(DataType.EmailAddress)]
        [IsEmailExist(ErrorMessage = "This e-mail address is already taken")]
        public string EmailID { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [DateOfBirthAdult(ErrorMessage = "You have to be an adult")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}