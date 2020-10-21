using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace VirtualPrinter
{
    class Program
    {

        private static IMqttClient _client;
        private static IMqttClientOptions _options;

        private static Printer _printer;
        private static string MY_LISTEN_TOPIC;
        private static string MY_PUBLISH_TOPIC;





        static void Main(string[] args)
        {
            _printer = new Printer();
            Console.WriteLine($"Printer {_printer.Id} is initialized and is {_printer.PrinterStatus}");

           var MY_ID = Environment.GetEnvironmentVariable("MY_ID");
           MY_LISTEN_TOPIC = Environment.GetEnvironmentVariable("MY_LISTEN_TOPIC");
           MY_PUBLISH_TOPIC = Environment.GetEnvironmentVariable("MY_PUBLISH_TOPIC");

           Console.WriteLine("************************************************");
           Console.WriteLine($"My_LISTEN_TOPIC: {MY_LISTEN_TOPIC}");
           Console.WriteLine($"My_PUBLISH_TOPIC: {MY_PUBLISH_TOPIC}");
           Console.WriteLine("************************************************");

            #region MQTT Setup
            //create MQTT client
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            //configure options
            _options = new MqttClientOptionsBuilder()
                .WithClientId($"{_printer.Id}")
                .WithTcpServer("dotnetbroker", 1884)
                .WithCredentials($"{_printer.Id}", "%spencer%")
                .WithCleanSession()
                .Build();


            //Handlers
            _client.UseConnectedHandler(e =>
            {
               //var topicString = $"devices\\printer\\{printer.Id}";

                //Subscribe to topic
                _client.SubscribeAsync(new TopicFilterBuilder().WithTopic(MY_LISTEN_TOPIC).Build()).Wait();

                Console.WriteLine($"{_printer.Id} Connected successfully with MQTT Brokers and subscribed to topic {MY_LISTEN_TOPIC}");
            });
            _client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine($"{_printer.Id} Disconnected from MQTT Brokers.");
            });


            _client.UseApplicationMessageReceivedHandler(e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"### {_printer.Id}  RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {payload}");
                Console.WriteLine();

                HandleMessage(payload);
            });

            #endregion

            //actually connect
            _client.ConnectAsync(_options).Wait();

            //keep the application running
            Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();
            _client.DisconnectAsync().Wait();
        }


        //TODO: Refactor for better handling
        private static void HandleMessage(string payload)
        {
            var messageToPublish = new MqttApplicationMessageBuilder()
                .WithTopic(MY_PUBLISH_TOPIC)
                .WithPayload($"{_printer.PrinterStatus}")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            switch (payload)
            {
                case "start":
                    _printer.Start();
                    _client.PublishAsync(MY_PUBLISH_TOPIC, Encoding.ASCII.GetBytes($"{_printer.PrinterStatus}")); //*client.PublishAsync(messageToPublish).Wait();
                    break;

                case "stop":
                    _printer.Stop();
                    _client.PublishAsync(MY_PUBLISH_TOPIC, Encoding.ASCII.GetBytes($"{_printer.PrinterStatus}"));
                    break;

                default:
                    _client.PublishAsync(MY_PUBLISH_TOPIC, Encoding.ASCII.GetBytes("UNKNOWN"));
                    break;
            }
        }
    }
}
