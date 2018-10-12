using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Roles")]
    public class RoleModel
    {
        [Key]
        public int ID { get; set; }

        public enum Role { Admin, User }

        public string Rolee { get; set; }

        //public virtual UserModel UserModel { get; set; }
    }
}