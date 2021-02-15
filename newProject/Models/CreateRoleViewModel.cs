using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace newProject.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
