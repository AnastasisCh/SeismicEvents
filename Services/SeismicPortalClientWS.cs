
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using SeismicEventsFireEvents.DTOs;

namespace SeismicEventsFireEvents.Services
{
    public class SeismicPortalClientWS : BackgroundService
    {
        private readonly Uri ServerUri;
        private readonly IConfiguration _configuration;
        public static ConcurrentQueue<SeismicPortalMessage> SeismicEvents = new ConcurrentQueue<SeismicPortalMessage>();

        public SeismicPortalClientWS(IConfiguration configuration)
        {
            _configuration = configuration;
            ServerUri = new Uri(_configuration.GetValue<string>("SeismicPortalWebSocketUri"));
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using ClientWebSocket ws = new ClientWebSocket();
            await ws.ConnectAsync(ServerUri, cancellationToken);
            byte[] buffer = new byte[8192];
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Server closed connection.");
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                try
                {
                    SeismicPortalMessage portalMessage = JsonConvert.DeserializeObject<SeismicPortalMessage>(message);
                    SeismicEvents.Enqueue(portalMessage);
                    Console.WriteLine($"Received seismic event: Magnitude {portalMessage.Data.Properties.Magnitude} at {portalMessage.Data.Properties.FlynnRegion}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Parse error: {ex.Message}");
                    Console.WriteLine($"Raw: {message}");
                }

            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.Write(cancellationToken.ToString());
        }
    }
}
