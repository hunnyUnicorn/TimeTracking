using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TimeTrackerAdmin.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static T ConvertToType<T>(this object value) where T : class
        {
            var jsonData = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}