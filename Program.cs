using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

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
            model.ExchangeDeclare("demoExchange-v1", ExchangeType.Direct);
            //model.ExchangeDeclare("demoExchange-Equalsv1", ExchangeType.Headers);

            //Create Queue in RabbitMQ 
            model.QueueDeclare("demoqueue-v1", true, false, false, null);
            Console.WriteLine("Creating Queue");

            // Bind Queue to Exchange
            model.QueueBind("demoqueue-v1", "demoExchange-v1", "directexchange_key");
            Console.WriteLine("Creating Binding");

            var properties = model.CreateBasicProperties();
            properties.Persistent = false;

            var json = JsonConvert.SerializeObject(new { name = "Jhon", age = 12, DateCreate = DateTime.Now });
            var body = Encoding.UTF8.GetBytes(json);

            //byte[] messagebuffer = Encoding.Default.GetBytes("Direct Message");
            model.BasicPublish("demoExchange-v1", "directexchange_key", properties, body);

            Console.WriteLine("Message Sent");


            //Read messages 
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (v, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            //autoAck: true - delete the messages from Queue
            model.BasicConsume(queue: "demoqueue-v1", autoAck: false, consumer: consumer);

            Console.ReadLine();
        }
    }
}
