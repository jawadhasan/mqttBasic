using System;
using System.Text;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace ConsolePublisher
{
    class Program
    {
        private static IMqttClient _client;
        private static IMqttClientOptions _options;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Publisher....");
            try
            {
                // Create a new MQTT client.
                var factory = new MqttFactory();
                _client = factory.CreateMqttClient();

                //configure options
                _options = new MqttClientOptionsBuilder()
                    .WithClientId("PublisherId")
                    .WithTcpServer("localhost", 1884)
                    .WithCredentials("bud", "%spencer%")
                    .WithCleanSession()
                    .Build();


                //handlers
                _client.UseConnectedHandler(e =>
                {
                    Console.WriteLine("Connected successfully with MQTT Brokers.");
                });
                _client.UseDisconnectedHandler(e =>
                {
                    Console.WriteLine("Disconnected from MQTT Brokers.");
                });
                _client.UseApplicationMessageReceivedHandler(e =>
                {
                    try
                    {
                        string topic = e.ApplicationMessage.Topic;
                        if (string.IsNullOrWhiteSpace(topic) == false)
                        {
                            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message, ex);
                    }
                });


                //connect
                _client.ConnectAsync(_options).Wait();

                Console.WriteLine("Press key to publish message.");
                Console.ReadLine();
                //simulating publish
                SimulatePublish();


                Console.WriteLine("Simulation ended! press any key to exit.");
                _client.DisconnectAsync().Wait();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //This method send messages to topic "test"
        static void SimulatePublish()
        {

            var counter = 0;
            while (counter < 10)
            {
                counter++;
                var testMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("test")
                    .WithPayload($"Payload: {counter}")
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();


                if (_client.IsConnected)
                {
                    Console.WriteLine($"publishing at {DateTime.UtcNow}");
                    _client.PublishAsync(testMessage);
                }
                Thread.Sleep(2000);
            }
        }
    }
}
