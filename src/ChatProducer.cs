using MassTransit;

namespace ChatApp
{
    public class ChatProducer
    {
        private readonly IBus _bus;

        public ChatProducer(IBus bus) => _bus = bus;

        public async Task SendMessageAsync(string message)
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:zi"));
            await sendEndpoint.Send(new ChatMessage(message));

            Console.WriteLine($"Message sent: {message}");
        }
    }
}