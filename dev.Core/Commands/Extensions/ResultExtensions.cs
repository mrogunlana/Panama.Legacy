using Models.Entities;
using System.Collections.Generic;

namespace Core.Entities
{
    public static class ResultExtensions
    {
        public static List<T> DataGet<T>(this IResult result)
        {
            return result.Data.DataGet<T>();
        }

        public static T DataGetSingle<T>(this IResult result)
        {
            return result.Data.DataGetSingle<T>();
        }
        public static List<T> KvpGet<T>(this IResult result, string key)
        {
            return result.Data.KvpGet<T>(key);
        }

        public static T KvpGetSingle<T>(this IResult result, string key)
        {
            return result.Data.KvpGetSingle<T>(key);
        }

        public static void RemoveAll<T>(this IResult result) where T : IModel
        {
            result.Data.RemoveAll<T>();
        }

        public static bool Exist<T>(this IResult result) where T : IModel
        {
            return result.Data.Exist<T>();
        }
    }
}
