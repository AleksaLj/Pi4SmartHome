using AdminManagementWebApi.Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementWebApi.Test.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminManagementDSLController : ControllerBase
    {
        private readonly ILogger<AdminManagementDSLController> Log;
        protected IMessageProducer MessageProducer { get; set; }
        protected IMessageConsumer MessageConsumer { get; set; }

        public AdminManagementDSLController(ILogger<AdminManagementDSLController> log, 
                                            IMessageProducer messageProducer, 
                                            IMessageConsumer messageConsumer)
        {
            Log = log;
            MessageProducer = messageProducer;
            MessageConsumer = messageConsumer;
        }

        [HttpPost]
        public async Task<IActionResult> SaveDSL([FromBody] AdminManagementDSLModel model)
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

                await MessageProducer.SendMessageAsync(model);

                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.Success = false;
                resultModel.Message = ex.Message;
                resultModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                resultModel.ReasonPhrase = "Internal Server Error.";

                throw;
            }            
        }
    }
}
