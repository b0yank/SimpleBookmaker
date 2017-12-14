namespace SimpleBookmaker.Web.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class SessionExtensions
    {
        public static void Set<TModel>(this ISession session, string key, TModel value)
        {
            var json = JsonConvert.SerializeObject(value);

            session.SetString(key, json);
        }

        public static TModel Get<TModel>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(TModel) :
            JsonConvert.DeserializeObject<TModel>(value);
        }
    }
}
