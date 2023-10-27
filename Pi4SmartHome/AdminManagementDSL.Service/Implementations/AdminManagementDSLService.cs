using AdminManagementDSL.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Service.Implementations
{
    public class AdminManagementDSLService : ServiceFabricHostedServiceBase, IAdminManagementDSLService
    {
        private readonly IAdminManagementDSLMsgHandler _msgHandler;
        private IMessageConsumer<AdminDSLMessage> _messageConsumer;
        private IMessageProducer<AdminDSLInterpreterEndMessage> _messageProducer;

        public AdminManagementDSLService(IServiceProvider serviceProvider, 
                                         ILogger<AdminManagementDSLService> log, 
                                         IAdminManagementDSLMsgHandler msgHandler, 
                                         IMessageConsumer<AdminDSLMessage> messageConsumer, 
                                         IMessageProducer<AdminDSLInterpreterEndMessage> messageProducer) : base(serviceProvider, log)
        {
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
               Log.LogError("Cannot process message.");
            }

            Log.LogInformation($"Finished execution {nameof(AdminManagementDSLService)}.{nameof(OnRunAsync)}.");
        }

        private async Task OnMessage(object? sender, AdminDSLMessage message)
        {
            //generate message for end of the inserting and interpreting AdminDsl Msg;
            var adminDslGuid = message.AdminDslGuid;

            var adminDslInterpreterEndMsg = new AdminDSLInterpreterEndMessage(adminDslGuid);

            if (!_messageProducer.IsConnected)
            {
                await _messageProducer.ConnectAsync();
            }

            await _messageProducer.SendMessageAsync(adminDslInterpreterEndMsg);
        }
    }
}
