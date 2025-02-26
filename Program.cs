using DotNetEnv;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            Env.Load();
            var rabbitMqConnection = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION");
            Console.WriteLine(rabbitMqConnection + "**********");
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                            x.AddConsumer<ChatConsumer>();

                            x.UsingRabbitMq((ctx, cfg) =>
                             {
                                cfg.Host(new Uri(rabbitMqConnection), h =>
                                {
                                    h.UseSsl(s => s.Protocol = System.Security.Authentication.SslProtocols.Tls12);
                                });
                                
                                cfg.ConfigureJsonSerializerOptions(options =>
                                {
                                    options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                                    options.WriteIndented = true;
                                    return options;
                                });

                                cfg.ReceiveEndpoint("zi", e =>
                                {
                                    e.ConfigureConsumer<ChatConsumer>(ctx);
                                });
                                cfg.ReceiveEndpoint("no", e =>
                                {
                                    e.ConfigureConsumer<ChatConsumer>(ctx);
                                });
                            });
                        });

                    services.AddMassTransitHostedService();
                    services.AddSingleton<ChatProducer>();
                })
                .Build();

            var producer = host.Services.GetRequiredService<ChatProducer>();
            await producer.SendMessageAsync(new ChatMessage("Hello, from Sebas!"));

            await host.RunAsync();
        }
    }
}
