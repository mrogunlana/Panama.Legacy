using dev.Core.Commands;
using dev.Core.Entities;
using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class GenerateUserId : ICommand
    {
        public void Execute(List<IModel> data)
        {
            var users = data.DataGet<User>();

            foreach (var user in users)
                user.ID = System.Guid.NewGuid();
        }
    }
}
