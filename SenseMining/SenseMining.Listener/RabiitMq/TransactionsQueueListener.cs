using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SenseMining.Domain.Services;
using SenseMining.Domain.TransactionsProcessing;

namespace SenseMining.Listener.RabiitMq
{
    public class TransactionsQueueListener : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqOptions _options;

        public TransactionsQueueListener(IServiceScopeFactory scopeFactory, RabbitMqOptions options)
        {
            _scopeFactory = scopeFactory;
            _options = options;

            var factory = new ConnectionFactory
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password
            };

            _connection =  factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StartListen()
        {
            _channel.ExchangeDeclare(exchange: _options.ExchangeName,
                type: ExchangeType.Direct,
                durable: true);

            _channel.QueueDeclare(queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(_options.QueueName, _options.ExchangeName, _options.QueueName, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, args) => Handle(args).Wait();

            _channel.BasicConsume(queue: _options.QueueName,
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
                    var handler = provider.GetRequiredService<ITransactionsConsumer>();

                    var messageObject = MessagePackSerializer.Deserialize<List<string>>(args.Body,
                        MessagePack.Resolvers.ContractlessStandardResolver.Instance);

                    await handler.ReceiveTransaction(messageObject);

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
