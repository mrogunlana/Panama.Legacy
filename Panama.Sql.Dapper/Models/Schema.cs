using Panama.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panama.Sql.Dapper.Models
{
    public class Schema : IModel
    {
        public int _ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name { get; set; }
        public string ColumnName { get; set; }
        public Type Type { get; set; }
    }
}
