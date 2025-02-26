using System.Text.Json;
using MassTransit;

namespace ChatApp
{
    public class ChatConsumer : IConsumer<ChatMessage>
    {
        public Task Consume(ConsumeContext<ChatMessage> context)
        {
            string jsonMessage = JsonSerializer.Serialize(context.Message, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"Received message (JSON): {jsonMessage}");
            return Task.CompletedTask;
        }
    }
}