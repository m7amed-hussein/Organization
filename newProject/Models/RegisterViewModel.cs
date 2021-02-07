using System;
using System.ComponentModel.DataAnnotations;

namespace newProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string  Password{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage ="The password missmach")]
        public string ConfirmPassword { get; set; }
    }
}
