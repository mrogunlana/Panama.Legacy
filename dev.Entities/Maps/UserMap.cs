using DapperExtensions.Mapper;
using dev.Entities.Models;

namespace dev.Entities.Maps
{
    public class UserMap : ClassMapper<User>
    {
        public UserMap()
        {
            Table("User");

            Map(x => x._ID).Key(KeyType.Assigned);
            Map(x => x.Created).ReadOnly();

            AutoMap();
        }
    }
}