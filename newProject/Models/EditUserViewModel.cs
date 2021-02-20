using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace newProject.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }
        public List<String> Roles { get; set; }
        public List<String> Claims { get; set; }
    }
}
