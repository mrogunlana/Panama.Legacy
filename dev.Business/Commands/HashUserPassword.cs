using Autofac.Features.AttributeFilters;
using dev.Core.Commands;
using dev.Core.Entities;
using dev.Core.Security;
using dev.Core.Security.Interfaces;
using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class HashUserPassword : ICommand
    {
        private IStringEncryptor _encryptor;
        public HashUserPassword([KeyFilter(nameof(AESEncryptor))] IStringEncryptor encryptor)
        {
            _encryptor = encryptor;
        }
        public void Execute(List<IModel> data)
        {
            var users = data.Get<User>();

            foreach (var user in users)
                user.Password = _encryptor.ToString(user.Password);
        }
    }
}
