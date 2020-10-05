using Newtonsoft.Json;
using System;

namespace GOOM.TR.MyRetail.NET.Tests.Helpers
{

    public static class JsonExt
    {
        public static string ToJson<T>(this T @object)
        {
            return JsonConvert.SerializeObject(@object, Startup.JsonSerializerSettings);
        }

        public static T FromJson<T>(this string serializedObject)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(serializedObject, Startup.JsonSerializerSettings);
            }
            catch (Exception ex)
            {
                var message = $"Unable to deserialize {typeof(T).FullName} from: {Environment.NewLine} {serializedObject}";
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
