// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;

Console.WriteLine("Hello, I Am Producer!");


var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
var connection = connectionFactory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare("logs", type: ExchangeType.Fanout);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(exchange: "logs",
                        routingKey: string.Empty,
                        basicProperties: null,
                        body: body);
Console.WriteLine($" [x] Sent {message}");
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
}

