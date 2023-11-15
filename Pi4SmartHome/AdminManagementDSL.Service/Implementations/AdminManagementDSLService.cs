using AdminManagementDSL.Application.Device.Queries;
using AdminManagementDSL.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Service.Implementations
{
    public class AdminManagementDSLService : ServiceFabricHostedServiceBase, IAdminManagementDSLService
    {
        private readonly IMediator _mediator;
        private readonly IAdminManagementDSLMsgHandler _msgHandler;
        private readonly IMessageConsumer<AdminDSLMessage> _messageConsumer;
        private readonly IMessageProducer<AdminDSLInterpreterEndMessage> _messageProducer;

        public AdminManagementDSLService(IServiceProvider serviceProvider, 
                                         ILogger log, 
                                         IMediator mediator,
                                         IAdminManagementDSLMsgHandler msgHandler, 
                                         IMessageConsumer<AdminDSLMessage> messageConsumer, 
                                         IMessageProducer<AdminDSLInterpreterEndMessage> messageProducer) : base(serviceProvider, log)
        {
            _mediator = mediator;
            _msgHandler = msgHandler;
            _messageConsumer = messageConsumer;
            _messageProducer = messageProducer;
        }

        protected override void OnAbort()
        {
            Log.LogInformation($"Execute {nameof(AdminManagementDSLService)}.{nameof(OnAbort)}.");
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(AdminManagementDSLService)}.{nameof(OnOpenAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(AdminManagementDSLService)}.{nameof(OnCloseAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnRunAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Started execution {nameof(AdminManagementDSLService)}.{nameof(OnRunAsync)}.");

            _messageConsumer.AddMessageEventHandler(_msgHandler);

            try
            {
                if (!_messageConsumer.IsConnected)
                {
                    await _messageConsumer.ConnectAsync();
                }
                _messageConsumer.OnMessageReceivedEvent += OnMessage;

                await _messageConsumer.Subscribe();
            }
            catch (Exception ex)
            {
               Log.LogError(ex, "Cannot process message. Error message: {0x1}", ex.Message);
            }

            Log.LogInformation($"Finished execution {nameof(AdminManagementDSLService)}.{nameof(OnRunAsync)}.");
        }

        private async Task OnMessage(object? sender, AdminDSLMessage message)
        {
            var adminDslGuid = message.AdminDslGuid;

            var getDevicesForIoTHubQuery = new GetDevicesForIoTHubQuery(adminDslGuid.ToString());
            var addIoTDeviceModels = await _mediator.Send(getDevicesForIoTHubQuery);

            var adminDslInterpreterEndMsg = new AdminDSLInterpreterEndMessage(adminDslGuid, addIoTDeviceModels.Select(item => item.DeviceIoTHubId));

            if (!_messageProducer.IsConnected)
            {
                await _messageProducer.ConnectAsync();
            }

            await _messageProducer.SendMessageAsync(adminDslInterpreterEndMsg);
        }
    }
}
