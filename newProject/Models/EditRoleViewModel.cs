using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace newProject.Models
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        [Required(ErrorMessage ="Role Name is Required")]
        public string RoleName { get; set; }

        public string Id { get; set; }
        public List<String> Users { get; set; }

    }
}
