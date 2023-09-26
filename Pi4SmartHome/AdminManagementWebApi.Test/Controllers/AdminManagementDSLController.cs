using AdminManagementWebApi.Test.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using System.Text.Json;

namespace AdminManagementWebApi.Test.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminManagementDSLController : ControllerBase
    {
        private readonly ILogger<AdminManagementDSLController> Log;
        protected IMessageProducer<AdminDSLMessage> MessageProducer { get; set; }
        private readonly IMediator _mediator;

        public AdminManagementDSLController(ILogger<AdminManagementDSLController> log, 
                                            IMessageProducer<AdminDSLMessage> messageProducer,
                                            IMediator mediator)
        {
            Log = log;
            MessageProducer = messageProducer;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdminManagementDsl([FromBody] AdminManagementDSLModel model)
        {
            var resultModel = new ResultModel();

            var json = PrepareJson();
            model.DSLSourceCode = json;

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

                var adminDslMsg = new AdminDSLMessage(model.DSLSourceCode);
                await MessageProducer.SendMessageAsync(adminDslMsg);

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

        private static string PrepareJson()
        {
            string adminDSL = @"
                                    PI4SMARTHOMEADMIN.PROVISION

                                    BEGIN
                                    {
	                                    DEFINE ESTATE: TABLE
	                                    EstateType: FIELD = `Home` AND Name: FIELD = `testName` AND Addr: FIELD = `testAddr` AND Description: FIELD = `testDesc`;

	                                    DEFINE ESTATE_USERS: TABLE
	                                    Users: AGGR = `user1@gmail.com, user2@gmail.com, user3@gmail.com`;

	                                    DEFINE ESTATE_PARTS: TABLE
	                                    EstateParts: AGGR = `Part1, Part2, Part3`;

	                                    DEFINE ESTATE_DEVICES: TABLE
	                                    DeviceType: FIELD = `devType1` AND IsActive: FIELD = `true` AND EstatePart: FIELD = `estatePart1`;
                                    }
                                    END
                               ";

            var model = new AdminManagementDSLModel { DSLSourceCode = adminDSL };
            var json = JsonSerializer.Serialize(model);

            return json;
        }
    }
}
