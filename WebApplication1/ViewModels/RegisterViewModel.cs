using System.ComponentModel.DataAnnotations;
/*
 * AK: Nie powinienes miec Modeli z bazy w jednym miejscu z ViewModel //KS: Poprawione
 */
namespace WebApplication1.ViewModels
{
    public class RegisterViewModel
    { 
        [Display(Name = "User Name: ")]
        [Required(ErrorMessage = "You have to get user name!")]
        public string UserName { get; set; }

        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "You have to get password! ")]
        [StringLength(16, ErrorMessage = "Password must have 6-16 characters length.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Email Address:")]
        [Required(ErrorMessage = "You have to get email address!")]
        [EmailAddress]
        public string Email { get; set; }

    }
}