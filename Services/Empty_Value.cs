using System.Text;
using MQTTnet;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Services
{
    public class Empty_Value
    {
        //public async Task<EmptyValue_Model> ReadValues_Empty()
        //{
        //    var data = new EmptyValue_Model();

        //    var factory = new MqttClientFactory();
        //    var client = factory.CreateMqttClient();

        //    var options = new MqttClientOptionsBuilder()
        //        .WithClientId("SCADAReceiver")
        //        .WithTcpServer("localhost", 1883)
        //        .Build();

        //    await client.ConnectAsync(options);

        //    client.ApplicationMessageReceivedAsync += e =>
        //    {
        //        var topic = e.ApplicationMessage.Topic;

        //        var value = Encoding.UTF8.GetString(
        //            e.ApplicationMessage.Payload.FirstSpan);

        //        switch (topic)
        //        {
        //            case "emptyvalue/read/112":
        //                data.Agg = value;
        //                break;

        //            case "emptyvalue/read/114":
        //                data.Cem = value;
        //                break;

        //            case "emptyvalue/read/428":
        //                data.Water = value;
        //                break;

        //            case "emptyvalue/read/116":
        //                data.Admix = value;
        //                break;

        //            case "emptyvalue/read/430":
        //                data.Ice = value;
        //                break;
        //        }

        //        return Task.CompletedTask;
        //    };

        //    await client.SubscribeAsync(
        //        new MqttTopicFilterBuilder()
        //            .WithTopic("emptyvalue/read/#")
        //            .Build());

        //    await Task.Delay(3000);

        //    await client.DisconnectAsync();

        //    return data;
        //}
        public async Task WriteEMPTY_112(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("emptyvalue/write/112")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteEMPTY_114(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("emptyvalue/write/114")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteEMPTY_116(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("emptyvalue/write/116")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteEMPTY_428(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("emptyvalue/write/428")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteEMPTY_430(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("emptyvalue/write/430")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }
    }
}