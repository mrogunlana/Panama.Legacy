using System.Collections.Generic;
using System.Linq;

namespace dev.Entities.Models
{
    public static class ModelExtensions
    {
        public static List<T> Get<T>(this List<IModel> data) where T : IModel
        {
            var result = new List<T>();
            foreach (var model in data)
                if (model is T)
                    result.Add((T)model);
            return result;
        }

        public static T GetSingle<T>(this List<IModel> data) where T : IModel
        {
            return data.Get<T>().FirstOrDefault();
        }

        public static void RemoveAll<T>(this List<IModel> data) where T : IModel
        {
            var delete = data.Get<T>();
            foreach (var deleted in delete)
                data.Remove(deleted);
        }

        public static bool Exist<T>(this List<IModel> data) where T : IModel
        {
            var exist = data.Get<T>();
            if (exist.Count == 0)
                return false;

            return true;
        }
    }
}
