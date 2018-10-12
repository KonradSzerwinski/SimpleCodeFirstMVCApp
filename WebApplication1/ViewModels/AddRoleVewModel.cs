using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class AddRoleVewModel
    {

        [Display(Name = "Role: ")]
        [Required(ErrorMessage = "Role name is required!")]
        public string RoleName { get; set; }

        //public int RoleID { get; set; }
    }
}