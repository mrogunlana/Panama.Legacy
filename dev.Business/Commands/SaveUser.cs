using dev.Core.Commands;
using dev.Core.Entities;
using dev.Core.Sql;
using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Business.Commands
{
    public class SaveUser : ICommand
    {
        private IQuery _query;
        public SaveUser(IQuery query)
        {
            _query = query;
        }
        public void Execute(List<IModel> data)
        {
            var users = data.Get<User>();

            foreach (var user in users)
                _query.Save(user, new { user.ID });
        }
    }
}
