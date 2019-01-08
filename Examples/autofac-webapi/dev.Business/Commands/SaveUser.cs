using dev.Entities.Models;
using Panama.Commands;
using Panama.Entities;
using Panama.Sql;
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
            var users = data.DataGet<User>();
            
            foreach (var user in users)
                _query.Save(user, new { user.ID });
        }
    }
}
