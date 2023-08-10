using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebPulse2023.Hubs
{
    public class WebPingHub : Hub
    {
        public async Task SendPingData(string websiteName, int responseTime)
        {
            await Clients.All.SendAsync("ReceivePingData", websiteName, responseTime);
        }
    }
}
