using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Text;
using System.Text.Json;

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

        //public async Task WriteSTEP_386(string value)
        //{
        //    var factory = new MqttClientFactory();
        //    var client = factory.CreateMqttClient();

        //    var options = new MqttClientOptionsBuilder()
        //        .WithTcpServer("localhost", 1883)
        //        .Build();

        //    await client.ConnectAsync(options);

        //    var message = new MqttApplicationMessageBuilder()
        //        .WithTopic("steptime/write/386")
        //        .WithPayload(value)
        //        .Build();

        //    await client.PublishAsync(message);

        //    await client.DisconnectAsync();
        //}

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

            await client.SubscribeAsync("stepTimeJson/read");

            client.ApplicationMessageReceivedAsync += e =>
            {
                string topic = e.ApplicationMessage.Topic;

                if (topic == "stepTimeJson/read")
                {
                    string json = Encoding.UTF8.GetString(
                        e.ApplicationMessage.Payload);

                    var data = JsonSerializer.Deserialize<Step_Time_Model>(json);

                    if (data != null)
                    {
                        _model.Step136 = data.Step136;
                        _model.Step140 = data.Step140;
                        _model.Step548 = data.Step548;
                        _model.Step552 = data.Step552;
                    }
                }

                return Task.CompletedTask;
            };

            await Task.Delay(3000);

            await client.DisconnectAsync();

            return _model;
        }
    }
}