using dev.Core.Entities;
using dev.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace dev.Core.Commands
{
    public static class HandlerExtensions
    {
        public static List<T> DataGet<T>(this List<IModel> data)
        {
            var result = new List<T>();
            foreach (var model in data)
                if (model?.GetType() == typeof(T))
                    result.Add((T)model);
            return result;
        }

        public static T DataGetSingle<T>(this List<IModel> data)
        {
            return data.DataGet<T>().FirstOrDefault();
        }
        public static List<T> KvpGet<T>(this List<IModel> data, string key)
        {
            return data
                .DataGet<KeyValuePair>()
                .Where(x => x.Key == key && x.Value is T)
                .Select(x => (T)x.Value)
                .ToList();
        }

        public static T KvpGetSingle<T>(this List<IModel> data, string key)
        {
            return data.KvpGet<T>(key).FirstOrDefault();
        }
    }
}
