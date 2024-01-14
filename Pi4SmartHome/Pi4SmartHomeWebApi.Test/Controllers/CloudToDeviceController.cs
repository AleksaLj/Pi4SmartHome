using Microsoft.AspNetCore.Mvc;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using Pi4SmartHomeWebApi.Test.Models;

namespace Pi4SmartHomeWebApi.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudToDeviceController : ControllerBase
    {
        private readonly ILogger<CloudToDeviceController> Log;
        private IMessageProducer<CloudToDeviceMessage> MessageProducer { get; set; }

        public CloudToDeviceController(ILogger<CloudToDeviceController> log, IMessageProducer<CloudToDeviceMessage> messageProducer)
        {
            Log = log;
            MessageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageToDevice([FromBody] Pi4SmartHomeDslModel model)
        {
            var resultModel = new ResultModel();

            var pi4SmartHomeDsl = GetPi4SmartHomeDslExample();
            model.DslSourceCode = pi4SmartHomeDsl;

            try
            {
                if (string.IsNullOrEmpty(model.DslSourceCode))
                {
                    resultModel.Success = false;
                    resultModel.ReasonPhrase = "DSL Source Code is null or empty.";
                    resultModel.Message = "DSL Source Code is null or empty.";
                    resultModel.StatusCode = System.Net.HttpStatusCode.BadRequest;

                    return BadRequest(resultModel);
                }

                if (!MessageProducer.IsConnected)
                {
                    await MessageProducer.ConnectAsync();
                }

                var cloudToDeviceMessage = new CloudToDeviceMessage(model.DslSourceCode);
                await MessageProducer.SendMessageAsync(cloudToDeviceMessage);

                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.Success = false;
                resultModel.Message = ex.Message;
                resultModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                resultModel.ReasonPhrase = "Internal Server Error.";
                Log.LogError(ex, $"[Server exception]: {ex.Message}");

                return BadRequest(resultModel);
            }
        }


        private static string GetPi4SmartHomeDslExample()
        {
            const string pi4SmartHomeDsl =
                @"
                SEND_DEVICE_MESSAGE
                To: `test-device-2`
                MessageBody: `Hello test-device-2!`
                MessageProperties:
                BEGIN
	                Prop1 = `test-device-2 prop1`,
	                Prop2 = `test-device-2 prop2`
                END
            ";

            return pi4SmartHomeDsl;
        }
    }
}
