using System;
using System.ComponentModel.DataAnnotations;
/*
 * AK: Nie powinienes miec Modeli z bazy w jednym miejscu z ViewModel //KS: Poprawione
 */
namespace WebApplication1.ViewModels
{
    public class AccountDetailsViewModel
    {
        [Display(Name = "Username: ")]
        public string UserName { get; set; }

        [Display(Name = "Email: ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Create Date: ")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Modification Date: ")]
        [DataType(DataType.DateTime)]
        public DateTime ModifyDate { get; set; }

        public int ID { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}