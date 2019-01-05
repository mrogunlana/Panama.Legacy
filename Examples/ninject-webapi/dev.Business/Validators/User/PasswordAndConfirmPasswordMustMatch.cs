using dev.Entities.Models;
using Panama.Commands;
using Panama.Entities;
using System.Collections.Generic;

namespace dev.Business.Validators
{
    public class PasswordAndConfirmPasswordMustMatch : IValidation
    {
        public bool IsValid(List<IModel> data)
        {
            var models = data.DataGet<User>();
            if (models == null)
                return false;

            foreach (var user in models)
                if (user.Password != user.ConfirmPassword)
                    return false;

            return true;
        }

        public string Message() => "Password and Confirm Password must match.";
    }
}
