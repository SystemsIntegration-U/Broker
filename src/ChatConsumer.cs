using MassTransit;

namespace ChatApp
{
    public class ChatConsumer : IConsumer<ChatMessage>
    {
        public Task Consume(ConsumeContext<ChatMessage> context)
        {
            Console.WriteLine($"Received message: {context.Message.Message}");
            return Task.CompletedTask;
        }
    }
}