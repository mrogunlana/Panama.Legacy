using System.Collections.Generic;

namespace Panama.Sql
{
    public interface IQuery
    {
        List<T> Get<T>(string sql, object parameters);
        void Insert<T>(T obj) where T : class;
        void Update<T>(T obj) where T : class;
        void Save<T>(T obj, object parameters) where T : class;
        bool Exist<T>(string sql, object parameters) where T : class;
        T GetSingle<T>(string sql, object parameters);
        void Delete<T>(T obj) where T : class;
        void Execute(string sql, object parameters);
    }
}
