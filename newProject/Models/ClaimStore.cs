using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace newProject.Models
{
    public static class ClaimStore
    {
        public static List<Claim> Claims = new List<Claim>(){
            new Claim("Create Role","Create Role"),
            new Claim("Edit Role","Edit Role"),
            new Claim("Delete Role","Delete Role")
        };
    }
}
