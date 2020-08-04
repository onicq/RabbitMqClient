
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var model = connection.CreateModel())
            {
                model.QueueDeclare("test-queue", durable: true, exclusive: false, autoDelete: false);

                var consumer = new EventingBasicConsumer(model);

                consumer.Received += (eventModel, args) =>
                {
                    var message = Encoding.UTF8.GetString(args.Body.ToArray());
                    System.Console.WriteLine(message);
                };

                model.BasicConsume(queue: "test-queue", autoAck: true, consumer);

                Console.ReadLine();
            }
        }
    }
}
