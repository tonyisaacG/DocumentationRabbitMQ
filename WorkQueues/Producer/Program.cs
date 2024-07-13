// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;

Console.WriteLine("Hello, I Am Producer!");



var connectionFactory = new ConnectionFactory(){HostName="localhost"};
var connection = connectionFactory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare("task_queue",durable:true, exclusive:false, autoDelete:false,arguments:null);
var message = GetMessage(args);
var messageStream = Encoding.UTF8.GetBytes(message);
    ///
    /// This QueueDeclare change needs to be applied to both the producer and consumer code. You also
    ///need to change the name of the queue for BasicConsume and BasicPublish .
    ///At this point we're sure that the task_queue queue won't be lost even if RabbitMQ restarts. Now we
    ///need to mark our messages as persistent.
    ///After the existing GetBytes, set IBasicProperties.Persistent to true :
    /// 
var properties = channel.CreateBasicProperties();
properties.Persistent = true;
channel.BasicPublish("","task_queue",properties,messageStream);
Console.WriteLine($" [x] Sent {message}");
Console.WriteLine(" Press [enter] to exit.");
Console.WriteLine($" [x] Received {message}");
int dots = message.Split('.').Length - 1;
Thread.Sleep(dots * 1000);
Console.WriteLine(" [x] Done");

Console.ReadLine();


static string GetMessage(string[] args)
{
  return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}
