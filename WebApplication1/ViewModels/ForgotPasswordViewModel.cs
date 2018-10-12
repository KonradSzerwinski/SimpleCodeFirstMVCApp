using System.ComponentModel.DataAnnotations;
/*
 * AK: Nie powinienes miec Modeli z bazy w jednym miejscu z ViewModel //KS: Poprawione
 */
namespace WebApplication1.ViewModels
{
    public class ForgotPasswordViewModel 
    {

        [Display(Name = "User name: ")]
        [Required(ErrorMessage = "Username is requied!")]
        public string UserName { get; set; }

        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }


    }
}