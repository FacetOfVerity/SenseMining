using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SenseMining.Domain.Services;

namespace SenseMining.Listener.RabiitMq
{
    public class TransactionsQueueListener : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public TransactionsQueueListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection =  factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StartListen()
        {
            var queueName = "SenseMining_Transactions";
            var exchangeName = "SenseMining";

            _channel.QueueDeclare(queue: "SenseMining_Transactions",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(queueName, exchangeName, queueName, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, args) => Handle(args).Wait();

            _channel.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer);

            Console.WriteLine("Прослушивание сообщений начато");
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        private async Task Handle(BasicDeliverEventArgs args)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var provider = scope.ServiceProvider;
                    var handler = provider.GetRequiredService<ITransactionsService>();

                    var messageObject = MessagePackSerializer.Deserialize<List<string>>(args.Body,
                        MessagePack.Resolvers.ContractlessStandardResolver.Instance);

                    await handler.InsertTransaction(messageObject);

                    _channel.BasicAck(args.DeliveryTag, false);
                }
                catch (Exception e)
                {
                     Console.WriteLine($"При обработке сообщения произошла ошибка: {e.Message}");
                    _channel.BasicNack(args.DeliveryTag, false, true);
                }
            }
        }
    }
}
