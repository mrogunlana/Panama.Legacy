using DapperExtensions.Mapper;
using dev.Entities.Models;

namespace dev.Entities.Maps
{
    public class UserMap : ClassMapper<User>
    {
        public UserMap()
        {
            Table("User");

            Map(x => x._ID).Key(KeyType.Identity);
            Map(x => x.ConfirmPassword).Ignore();
            Map(x => x.Created).ReadOnly();

            AutoMap();
        }
    }
}