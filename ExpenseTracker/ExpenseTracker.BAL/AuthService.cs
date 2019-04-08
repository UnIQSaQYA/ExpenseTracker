using ExpenseTracker.Helper;
using ExpenseTracker.Model;
using ExpensTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BAL
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService()
        {
            _authRepository = new AuthRepository();
        }

        public Message RegisterUser(RegisterViewModel registerViewModel)
        {
            Message model = new Message();
            try
            {
                registerViewModel.Status = true;
                registerViewModel.RoleId = 1;
                var register = registerViewModel.ToDalEntity();
                _authRepository.Add(register);
                _authRepository.SaveChanges();

                model.StatusCode = 200;
                model.Status = "User Registration Successful";

            }
            catch (Exception ex)
            {

                model.StatusCode = 200;
                model.Status = "User Registration Successful";
            }

            return model;
        }

        public bool CheckUserExists(string Email)
        {
            return _authRepository.FindBy(x => x.Email == Email.Trim()).Any();
        }

        public bool CheckOldPasswordIsCorrect(int userId, string password)
        {
            var data = _authRepository.FindBy(x => x.Id == userId).FirstOrDefault();
            return Authentication.CheckPass(password, data.Password);
        }

        public Message ChangePassword(ChangePasswordViewModel changePassword, int userId)
        {
            Message msg = new Message();
           
            try
            {
                var data = _authRepository.FindBy(x => x.Id == userId).FirstOrDefault();
                data.Password = Authentication.HashText(changePassword.NewPassword);
                _authRepository.Edit(data);
                _authRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Password Change Successfully!";
            }catch(Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "There was an erroe while changing password.";
            }
            return msg;
        }

        public bool CheckCorrectPassword(LoginViewModel model)
        {
            var data = _authRepository.FindBy(x => x.Email == model.Email).FirstOrDefault();
            if (data != null)
                return Authentication.CheckPass(model.Password, data.Password);
            return false;
        }

        public RegisterViewModel GetUserByEmail(string email)
        {
            var data = _authRepository.FindBy(x => x.Email == email).Select(x => new RegisterViewModel
            {
                Id = x.Id,
                Address = x.Address,
                Email = x.Email,
                Name = x.Name,
                Occupation = x.Occupation,
                RoleId = x.Roles.Id,
                CreatedAt = x.CreatedAt,
                Password = x.Password,
                Status = x.Status,
                RoleName = x.Roles.RoleName,
            }).FirstOrDefault();

            return data;
        }

    }
}

