using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class EditAccountViewModel
    {
        int _id;
        public int ID {
            get
            {
                return this._id;
            }

            set
            {
                if (value != 0)
                    this._id = value;
            }
        }

        [Display(Name = "Change Username: ")]
        [Required(ErrorMessage = "Username is required!")]
        public string NewUserName { get; set; }

        [Display(Name = "Change Email: ")]
        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }

        [Display(Name = "Change Role: ")]
        public string NewRole { get; set; }

        [Display(Name = "Change Password: ")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new Password: ")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

        [Display(Name = "Old password to save changes: ")]
        [Required(ErrorMessage = "Old Password is required to save changes")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Last modification: ")]
        [DataType(DataType.DateTime)]
        public DateTime ModifyDate { get; set; }

        [Display(Name = "Profile Picture: ")]
        public byte[] ProfilePicture { get; set; }



    }
}