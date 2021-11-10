using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement
{
    public static class SessionExtensions
    {
        public static void Put<T>(this ISession session, string key, T value) where T : class
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            byte[] output;
            if (session.TryGetValue(key, out output))
            {
                return JsonConvert.DeserializeObject<T>(session.GetString(key));
            }
            else
            {
                return null;
            }
        }
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
