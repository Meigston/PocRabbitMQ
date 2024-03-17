namespace PocRabbitMq.Workers
{
    using Microsoft.AspNetCore.Connections;
    using Microsoft.Extensions.Hosting;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class RabbitMQConsumerService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, VirtualHost = "/", UserName = "fjsadmin", Password = "Fujitsu@2022" }; // Change as needed

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare a topic type exchange
            _channel.ExchangeDeclare(exchange: "YOUR_exchange_topic_example", type: ExchangeType.Topic);

            // Declare a queue
            _queueName = _channel.QueueDeclare("YOUR_queue_topic_example").QueueName;

            // Create a binding between the exchange and the queue using a routing key
            string routingKey = "YOUR_example.topic"; // Change as needed
            _channel.QueueBind(queue: _queueName,
                               exchange: "YOUR_exchange_topic_example",
                               routingKey: routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                // Manual message acknowledgment
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: _queueName,
                                  autoAck: false,
                                  consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

}
