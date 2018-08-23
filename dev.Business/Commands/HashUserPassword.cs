using dev.Core.Commands;
using dev.Core.IoC;
using dev.Core.Security;
using dev.Core.Security.Interfaces;
using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class HashUserPassword : ICommand
    {
        private IStringEncryptor _encryptor;
        public HashUserPassword()
        {
            _encryptor = CompositionRoot.Resolve<IStringEncryptor>(nameof(AESEncryptor));
        }
        public void Execute(List<IModel> data)
        {
            var users = data.Get<User>();

            foreach (var user in users)
                user.Password = _encryptor.ToString(user.Password);
        }
    }
}
