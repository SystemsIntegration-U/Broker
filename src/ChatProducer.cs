using System.Text.Json;
using MassTransit;

namespace ChatApp
{
    public class ChatProducer
    {
        private readonly IBus _bus;

        public ChatProducer(IBus bus) => _bus = bus;

        public async Task SendMessageAsync(ChatMessage message)
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:zi"));
            await sendEndpoint.Send(message);

            string jsonMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"Message sent (JSON): {jsonMessage}");
        }
    }
}