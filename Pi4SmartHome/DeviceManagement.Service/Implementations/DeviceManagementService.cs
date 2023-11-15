using DeviceManagement.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace DeviceManagement.Service.Implementations
{
    public class DeviceManagementService : ServiceFabricHostedServiceBase, IDeviceManagementService
    {
        private readonly IMessageConsumer<AdminDSLInterpreterEndMessage> _messageConsumer;
        private readonly IAdminDSLInterpreterEndMsgHandler _msgHandler;

        public DeviceManagementService(IServiceProvider serviceProvider, 
                                       ILogger log,
                                       IMessageConsumer<AdminDSLInterpreterEndMessage> messageConsumer,
                                       IAdminDSLInterpreterEndMsgHandler msgHandler) : base(serviceProvider, log)
        {
            _messageConsumer = messageConsumer;
            _msgHandler = msgHandler;
        }

        protected override void OnAbort()
        {
            Log.LogInformation($"Execute {nameof(DeviceManagementService)}.{nameof(OnAbort)}.");
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(DeviceManagementService)}.{nameof(OnOpenAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(DeviceManagementService)}.{nameof(OnCloseAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnRunAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Started execution {nameof(DeviceManagementService)}.{nameof(OnRunAsync)}.");

            _messageConsumer.AddMessageEventHandler(_msgHandler);

            try
            {
                if (!_messageConsumer.IsConnected)
                {
                    await _messageConsumer.ConnectAsync();
                }

                //TO DO : Not scope of this project - call admin microservice and update admin devices table in db.
                //TO DO : In that case on admin part we will have last state and with that we will be able to activate/ deactivate/ delete devices (and maybe to extend AdminDSL).
                //_messageConsumer.OnMessageReceivedEvent += OnMessage;

                await _messageConsumer.Subscribe();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Cannot process message. Error message: {0x1}", ex.Message);
            }

            Log.LogInformation($"Finished execution {nameof(DeviceManagementService)}.{nameof(OnRunAsync)}.");
        }
    }
}
