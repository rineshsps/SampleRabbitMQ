using RabbitMQ.Client;
using System;

namespace SampleRabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            string UserName = "guest";
            string Password = "guest";
            string HostName = "localhost";
            //Main entry point to the RabbitMQ .NET AMQP client
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                UserName = UserName,
                Password = Password,
                HostName = HostName
            };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            Console.WriteLine("Creating Exchange");
            // Create Exchange
            model.ExchangeDeclare("demoExchange1", ExchangeType.Direct);
            model.ExchangeDeclare("demoExchange-Equalsv1", ExchangeType.Headers);
            Console.ReadLine();
        }
    }
}
