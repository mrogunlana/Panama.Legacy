using dev.Entities.Models;
using Ninject;
using Panama.Commands;
using Panama.Entities;
using Panama.Security;
using Panama.Security.Interfaces;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class HashUserPassword : ICommand
    {
        private IStringEncryptor _encryptor;
        public HashUserPassword([Named(nameof(AESEncryptor))] IStringEncryptor encryptor)
        {
            _encryptor = encryptor;
        }
        public void Execute(List<IModel> data)
        {
            var users = data.DataGet<User>();

            foreach (var user in users)
                user.Password = _encryptor.ToString(user.Password);
        }
    }
}
