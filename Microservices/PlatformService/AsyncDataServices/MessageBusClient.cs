using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()   {HostName = _configuration["RabbitMQHost"],
                                                    Port = int.Parse(_configuration["RabbitMQPort"])};
            try{
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
                Console.WriteLine("Connected to Message Bus");

            }catch(Exception ex){
                Console.WriteLine("Could not connect to Rabbit MQ : "+ex.Message);
            }
        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if(_connection.IsOpen){
                Console.WriteLine("RabbitQueue is up...Sending message...");
                SendMessage(message);
            }else{
                Console.WriteLine("RabbitQueue is shut...Not Sending message...");
            }
        }

        private void SendMessage(string message){
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange:"trigger",routingKey:"",basicProperties:null,body:body);
            Console.WriteLine("We have successfully sent the message "+message);
        }

        public void Dispose(){
            Console.WriteLine("Message Bus Dispose started");
            if(_channel.IsOpen){
                _channel.Close();
                _connection.Close();
            }
            Console.WriteLine("Message Bus Disposed");
        }
        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e){
            Console.WriteLine("Rabbit MQ connection is down");
        }
    }
}