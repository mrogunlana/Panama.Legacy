using dev.Entities.Models;
using Panama.Commands;
using Panama.Entities;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class RemoveUser : ICommand
    {
        public void Execute(List<Panama.Entities.IModel> data)
        {
            data.RemoveAll<User>();
        }
    }
}
