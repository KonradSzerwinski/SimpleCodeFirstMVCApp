using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 * AK: Nie powinienes miec Modeli z bazy w jednym miejscu z ViewModel
 */
namespace WebApplication1.Models
{
    [Table("Users")]
    public class UserModel 
    {
        [Key]
        public int ID { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Role { get; set; }

        public byte[] ProfilePicture { get; set; }

        public virtual RoleModel RoleModel { get; set; }

        
    }
}