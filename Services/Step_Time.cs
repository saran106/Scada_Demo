using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Text;

namespace Scada_Demo.Services
{
    public class Step_Time
    {
        private readonly Step_Time_Model _model = new();

        public async Task WriteSTEP_136(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("steptime/write/136")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteSTEP_548(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("steptime/write/548")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteSTEP_552(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("steptime/write/552")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteSTEP_386(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("steptime/write/386")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteSTEP_140(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("steptime/write/140")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteSTEP_410(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("steptime/write/410")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }
        public async Task<Step_Time_Model> ReadValue()
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            await client.SubscribeAsync("steptime/read/136");
            await client.SubscribeAsync("steptime/read/548");
            await client.SubscribeAsync("steptime/read/552");
            await client.SubscribeAsync("steptime/read/386");
            await client.SubscribeAsync("steptime/read/140");
            await client.SubscribeAsync("steptime/read/410");

            client.ApplicationMessageReceivedAsync += e =>
            {
                string topic = e.ApplicationMessage.Topic;

                string value = Encoding.UTF8.GetString(
     e.ApplicationMessage.Payload);

                switch (topic)
                {
                    case "steptime/read/136":
                        _model.Agg = value;
                        break;

                    case "steptime/read/548":
                        _model.Cem1 = value;
                        break;

                    case "steptime/read/552":
                        _model.Cem4 = value;
                        break;

                    case "steptime/read/386":
                        _model.Water = value;
                        break;

                    case "steptime/read/140":
                        _model.Admix1 = value;
                        break;

                    case "steptime/read/410":
                        _model.Silica = value;
                        break;
                }

                return Task.CompletedTask;
            };

            await Task.Delay(3000);

            await client.DisconnectAsync();

            return _model;
        }
    }
}