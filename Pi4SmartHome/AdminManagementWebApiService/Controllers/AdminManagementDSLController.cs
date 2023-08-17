using AdminManagementWebApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementWebApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminManagementDSLController : ControllerBase
    {
        private readonly ILogger<AdminManagementDSLController> Log;
        private readonly IMessageProducer MessageProducer;

        public AdminManagementDSLController(ILogger<AdminManagementDSLController> logger, IMessageProducer messageProducer)
        {
            Log = logger;
            MessageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<ActionResult<ResultModel>> SaveAdminManagementDSL([FromBody] AdminManagementDSLModel model)
        {
            var resultModel = new ResultModel();

            try
            {
                if (string.IsNullOrEmpty(model.DSLSourceCode))
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

                await MessageProducer.SendMessageAsync(resultModel);

                return Ok(model);
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
    }
}
