using Pi4SmartHome.Core.RabbitMQ.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IRabbitMQ
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        bool IsConnected { get; }
    }
}
