using System;
using System.ComponentModel.DataAnnotations;

namespace newProject.Utilities
{
    public class ValidEmailDomanAttributes : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomanAttributes(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            var splited = value.ToString().Split("@");
            return splited[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
