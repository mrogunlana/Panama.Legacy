using dev.Core.Commands;
using dev.Core.Entities;
using dev.Core.Sql;
using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class RemoveUser : ICommand
    {
        public void Execute(List<IModel> data)
        {
            data.RemoveAll<User>();
        }
    }
}
