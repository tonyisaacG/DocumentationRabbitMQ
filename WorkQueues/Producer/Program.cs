using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

class Producer
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, I Am Producer!");

        var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare("task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var message = GetMessage(args, random);
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", "task_queue", properties, messageBody);
                Console.WriteLine($" [x] Sent {message}");
                Console.WriteLine(" Press [enter] to exit.");
                Console.WriteLine($" [x] Received {message}");
                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);
                Console.WriteLine(" [x] Done");
            }
        }

        Console.ReadLine();
    }

    static string GetMessage(string[] args, Random random)
    {
        var num = random.Next(4); // Ensure it stays within bounds
        return "Hello World!" + new string('.', num);
    }
}
