using AdminManagementWebApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementWebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminManagementDSLController : ControllerBase
    {
        private readonly ILogger<AdminManagementDSLController> Log;
        protected IMessageProducer<AdminDSLMessage> MessageProducer { get; set; }

        public AdminManagementDSLController(ILogger<AdminManagementDSLController> log,
                                            IMessageProducer<AdminDSLMessage> messageProducer)
        {
            Log = log;
            MessageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdminManagementDsl([FromBody] AdminManagementDSLModel model)
        {
            var resultModel = new ResultModel();

            var msg = PrepareMsg();
            model.DSLSourceCode = msg;

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

                var adminDslMsg = new AdminDSLMessage(model.DSLSourceCode, adminDslGuid: Guid.NewGuid());
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

        private static string PrepareMsg()
        {
            string adminDSL = @"
                                    PI4SMARTHOMEADMIN.PROVISION

                                    BEGIN
                                    {
	                                    DEFINE ESTATE: TABLE
	                                    EstateType: FIELD = `Home` AND Name: FIELD = `testName` AND Addr: FIELD = `testAddr` AND Description: FIELD = `testDesc`;

	                                    DEFINE ESTATE_USERS: TABLE
	                                    Users: AGGR = `aleksaljujic97@gmail.com, test@gmail.com`;

	                                    DEFINE ESTATE_PARTS: TABLE
	                                    EstateParts: AGGR = `LivingRoom, Bedroom`;

	                                    DEFINE ESTATE_DEVICES: TABLE
	                                    DeviceType: FIELD = `LightSensor` AND IsActive: FIELD = `true` AND EstatePart: FIELD = `LivingRoom`;
                                    }
                                    END
                               ";

            //var model = new AdminManagementDSLModel { DSLSourceCode = adminDSL };
            //var json = JsonSerializer.Serialize(model);

            return adminDSL;
        }
    }
}
