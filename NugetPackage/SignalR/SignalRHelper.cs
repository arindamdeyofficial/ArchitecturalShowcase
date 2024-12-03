using CustomLoggerHelper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace SignalR
{
    public class SignalRHub : Hub
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        public SignalRHub(ILoggerHelper logger, IConfiguration configRoot)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // This method can notify clients, e.g., when a payment is processed.
        public async Task SendNotification(string notification)
        {
            await Clients.All.SendAsync("ReceiveNotification", notification);
        }
    }
}
