using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using newProject.Utilities;

namespace newProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmail",controller:"account")]
        [ValidEmailDomanAttributes(allowedDomain:"m.com"
            ,ErrorMessage = "Domail Name must be :m.com")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string  Password{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage ="The password missmach")]
        public string ConfirmPassword { get; set; }


        public string City { get; set; }
    }
}
