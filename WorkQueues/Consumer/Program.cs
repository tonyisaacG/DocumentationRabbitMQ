// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, I Am Consumer!");


var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
var connection = connectionFactory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare("task_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
Console.WriteLine(" [*] Waiting for messages.");
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    int dots = message.Split('.').Length - 1;
    Thread.Sleep(dots * 1000);
    Console.WriteLine(" [x] Done");
    /// <summary>
    /// Manual message acknowledgments are turned on by default. In previous examples we explicitly
    ///turned them off by setting the autoAck ("automatic acknowledgement mode") parameter to true. It's
    ///time to remove this flag and manually send a proper acknowledgment from the worker, once we're
    ///done with a task.
    /// </summary>
    channel.BasicAck(deliveryTag:ea.DeliveryTag, multiple: false) ;
};
channel.BasicConsume(queue: "task_queue", autoAck: false, consumer: consumer);
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
