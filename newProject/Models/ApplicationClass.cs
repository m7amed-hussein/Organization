using System;
using Microsoft.AspNetCore.Identity;

namespace newProject.Models
{
    public class ApplicationClass:IdentityUser
    {
        public string city  { get; set; }
        public ApplicationClass()
        {
        }
    }
}
