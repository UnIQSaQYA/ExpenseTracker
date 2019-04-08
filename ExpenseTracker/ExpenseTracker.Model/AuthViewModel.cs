using ExpenseTracker.Helper;
using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class RegisterViewModel: User
    {
        public RegisterViewModel()
           : base()
        {
        }
        public Users ToDalEntity()
        {
            return ToDalEntity(new Users());
        }

        public Users ToDalEntity(Users user)
        {
            user.Id = this.Id;
            user.Name = this.Name;
            user.Occupation = this.Occupation;
            user.RoleId = this.RoleId;
            user.Email = this.Email;
            user.Address = this.Address;
            user.Password = Authentication.HashText(this.Password);
            user.Status = this.Status;
            user.CreatedAt = DateTime.Now;
            return user;
        }
        [Required(ErrorMessage = "This Field is required.")]
        public new string Name { get; set; }
        [Required(ErrorMessage = "This Field is Required.")]
        public new string Email { get; set; }
        [Required(ErrorMessage = "This Field is Required.")]
        public new string Password { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Confirmation password do not match.")]
        public new string ConfirmPassword { get; set; }
        public bool Terms { get; set; }
    }

    public class LoginViewModel
    {
        public LoginViewModel()
          : base()
        {
        }
        public Users ToDalEntity()
        {
            return ToDalEntity(new Users());
        }

        public Users ToDalEntity(Users user)
        {
            user.Email = this.Email;
            user.Password = this.Password;
            return user;
        }
        [Required(ErrorMessage = "This Field is Required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This Field is Required.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel()
         : base()
        {
        }

        [Required(ErrorMessage = "This Field is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        [NotMapped]
        [Compare("NewPassword", ErrorMessage = "Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
