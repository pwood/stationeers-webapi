using System.IO;
using System.Threading.Tasks;
using Ceen;
using Newtonsoft.Json;

namespace WebAPI
{
    public static class HTTPContextUtils
    {
        public static T ParseBody<T>(this IHttpContext context)
        {
            var body = context.Request.Body;
            var reader = new StreamReader(body, System.Text.Encoding.UTF8);
            var text = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(text);

        }
        public static Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, object jsonBody)
        {
            var jsonText = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
            return HTTPContextUtils.SendResponse(context, statusCode, "application/json", jsonText);
        }

        public static async Task SendResponse(this IHttpContext context, HttpStatusCode statusCode, string contentType = null, string body = null)
        {
            var response = context.Response;

            response.StatusCode = statusCode;

            if (contentType != null)
            {
                response.Headers.Add("Content-Type", contentType);
            }

            if (contentType == "application/json")
            {
                await response.WriteAllJsonAsync(body);
            }
            else
            {
                // Why the fuck isnt this working
                // if (body != null)
                // {
                //     var stream = new MemoryStream();
                //     var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
                //     writer.Write(body);
                //     await response.WriteAllAsync(stream, contentType);
                // }
                await response.WriteAllJsonAsync(body);
            }


        }
    }
}