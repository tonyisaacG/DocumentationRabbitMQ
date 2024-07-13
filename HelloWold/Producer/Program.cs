// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;

Console.WriteLine("Hello, I Am Producer!");


var connectionFactory = new ConnectionFactory(){HostName="localhost"};
var connection = connectionFactory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare("HelloWorld",durable:false, exclusive:false, autoDelete:false,arguments:null);
var message  = "Hello this First App With RabbitMQ, I Am Producter This My Job";
var messageStream = Encoding.UTF8.GetBytes(message);
channel.BasicPublish("","HelloWorld",null,messageStream);
Console.WriteLine($" [x] Sent {message}");
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
