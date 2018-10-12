using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class UserListViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name ="Create Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Modify Date")]
        [DataType(DataType.DateTime)]
        public DateTime ModifyDate { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        
        public int ID { get; set; }


    }
}