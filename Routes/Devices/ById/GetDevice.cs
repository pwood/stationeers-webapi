
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Objects.Pipes;
using Ceen;
using WebAPI.Authentication;
using WebAPI.Payloads;

namespace WebAPI.Routes.Devices.ById
{
    class GetDevice : IWebRoute
    {
        public string Method => "GET";

        public string[] Segments => new[] { "devices", ":deviceId" };

        public async Task OnRequested(IHttpContext context, IDictionary<string, string> pathParams)
        {
            Authenticator.VerifyAuth(context);

            // TODO: Return UNPROCESSABLE_ENTITY if deviceId invalid.
            var referenceId = long.Parse(pathParams["deviceId"]);
            var device = await Dispatcher.RunOnMainThread(() => Device.AllDevices.Find(x => x.ReferenceId == referenceId));
            if (device == null)
            {
                await context.SendResponse(HttpStatusCode.NotFound, new ErrorPayload()
                {
                    message = "Device not found."
                });
                return;
            }

            await context.SendResponse(HttpStatusCode.OK, DevicePayload.FromDevice(device));
        }
    }
}