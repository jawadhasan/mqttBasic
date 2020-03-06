using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace ConsoleSubscriber
{
    class Program
    {
      
        private static IMqttClient _client;
        private static IMqttClientOptions _options;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting Subsriber....");
                
                //create subscriber client
                var factory = new MqttFactory();
                _client = factory.CreateMqttClient();

                //configure options
                _options = new MqttClientOptionsBuilder()
                    .WithClientId("SubscriberId")
                    .WithTcpServer("dotnetbroker", 1884)
                    .WithCredentials("bud", "%spencer%")
                    .WithCleanSession()
                    .Build();

                //Handlers
                _client.UseConnectedHandler(e =>
                {
                    Console.WriteLine("Connected successfully with MQTT Brokers.");

                    //Subscribe to topic
                    _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("test").Build()).Wait();
                });
                _client.UseDisconnectedHandler(e =>
                {
                    Console.WriteLine("Disconnected from MQTT Brokers.");
                });
                _client.UseApplicationMessageReceivedHandler(e =>
                {
                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                    Console.WriteLine();

                  //  Task.Run(() => _client.PublishAsync("hello/world"));
                });

                //actually connect
                _client.ConnectAsync(_options).Wait();

                Console.WriteLine("Press key to exit");
                Console.ReadLine();

                //To keep the app running in container
                //https://stackoverflow.com/questions/38549006/docker-container-exits-immediately-even-with-console-readline-in-a-net-core-c
                Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();
                _client.DisconnectAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }
    }
}
