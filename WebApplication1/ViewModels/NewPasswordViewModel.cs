using System.ComponentModel.DataAnnotations;
/*
 * AK: Nie powinienes miec Modeli z bazy w jednym miejscu z ViewModel //KS: Poprawione
 */
namespace WebApplication1.ViewModels
{
    public class NewPasswordViewModel
    {
        [Display(Name = "New Password: ")]
        [Required(ErrorMessage = "You have to get password! ")]
        [StringLength(16, ErrorMessage = "Password must have 6-16 characters length.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password: ")]
        [Required(ErrorMessage = "You have to get password! ")]
        [StringLength(16, ErrorMessage = "Password must have 6-16 characters length.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}