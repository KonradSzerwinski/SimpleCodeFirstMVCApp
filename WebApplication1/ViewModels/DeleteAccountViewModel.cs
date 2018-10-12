using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class DeleteAccountViewModel
    {
        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int ID { get; set; }
    }
}