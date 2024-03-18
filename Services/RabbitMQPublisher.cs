namespace PocRabbitMq.Services
{
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using System.Text;
    using System.Text.Json;

    public class RabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, VirtualHost = "/", UserName = "admin", Password = "admin@123" }; // Change as needed
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare a exchange if not exists
            //_channel.ExchangeDeclare(exchange: "YOUR_exchange_topic_example", type: ExchangeType.Topic);
        }

        public void PublishMessage(string routingKey, object message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(exchange: "YOUR_exchange_topic_example",
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            Console.WriteLine(" [x] Sent {0}:{1}", routingKey, message);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }

}
