using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Text;

namespace Scada_Demo.Services
{
    public class JogTime
    {
        private readonly JogTime_Model _model = new();

        public async Task WriteJOGON_142(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/142")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_146(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/146")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_150(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/150")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_154(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/154")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_236(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/236")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_240(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/240")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_416(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/416")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGON_420(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogon/write/420")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGOFF_144(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogoff/write/144")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGOFF_148(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogoff/write/148")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGOFF_152(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogoff/write/152")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteJOGOFF_156(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("jogoff/write/156")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        //public async Task<JogTime_Model> ReadValue()
        //{
        //    var factory = new MqttClientFactory();
        //    var client = factory.CreateMqttClient();

        //    var options = new MqttClientOptionsBuilder()
        //        .WithTcpServer("localhost", 1883)
        //        .Build();

        //    await client.ConnectAsync(options);

        //    await client.SubscribeAsync("jogon/read/142");
        //    await client.SubscribeAsync("jogon/read/146");
        //    await client.SubscribeAsync("jogon/read/150");
        //    await client.SubscribeAsync("jogon/read/154");
        //    await client.SubscribeAsync("jogon/read/236");
        //    await client.SubscribeAsync("jogon/read/240");
        //    await client.SubscribeAsync("jogon/read/416");
        //    await client.SubscribeAsync("jogon/read/420");

        //    await client.SubscribeAsync("jogoff/read/144");
        //    await client.SubscribeAsync("jogoff/read/148");
        //    await client.SubscribeAsync("jogoff/read/152");
        //    await client.SubscribeAsync("jogoff/read/156");

        //    client.ApplicationMessageReceivedAsync += e =>
        //    {
        //        string topic = e.ApplicationMessage.Topic;

        //        string value = Encoding.UTF8.GetString(
        //            e.ApplicationMessage.Payload);

        //        switch (topic)
        //        {
        //            case "jogon/read/142":
        //                short.TryParse(value, out short agg1On);
        //                _model.Agg1On = agg1On;
        //                break;

        //            case "jogon/read/146":
        //                short.TryParse(value, out short agg2On);
        //                _model.Agg2On = agg2On;
        //                break;

        //            case "jogon/read/150":
        //                short.TryParse(value, out short agg3On);
        //                _model.Agg3On = agg3On;
        //                break;

        //            case "jogon/read/154":
        //                short.TryParse(value, out short agg4On);
        //                _model.Agg4On = agg4On;
        //                break;

        //            case "jogon/read/236":
        //                short.TryParse(value, out short cem3On);
        //                _model.Cem3On = cem3On;
        //                break;

        //            case "jogon/read/240":
        //                int.TryParse(value, out int adm1On);
        //                _model.Adm1On = adm1On;
        //                break;

        //            case "jogon/read/416":
        //                int.TryParse(value, out int iceOn);
        //                _model.IceOn = iceOn;
        //                break;

        //            case "jogon/read/420":
        //                int.TryParse(value, out int waterOn);
        //                _model.WaterOn = waterOn;
        //                break;

        //            case "jogoff/read/144":
        //                short.TryParse(value, out short agg1Off);
        //                _model.Agg1Off = agg1Off;
        //                break;

        //            case "jogoff/read/148":
        //                short.TryParse(value, out short agg2Off);
        //                _model.Agg2Off = agg2Off;
        //                break;

        //            case "jogoff/read/152":
        //                short.TryParse(value, out short agg3Off);
        //                _model.Agg3Off = agg3Off;
        //                break;

        //            case "jogoff/read/156":
        //                short.TryParse(value, out short agg4Off);
        //                _model.Agg4Off = agg4Off;
        //                break;
        //        }

        //        return Task.CompletedTask;
        //    };

        //    await Task.Delay(3000);

        //    await client.DisconnectAsync();

        //    return _model;
        //}

        //public async Task<JogTime_Model> ReadValues_Agg()
        //{
        //    return await ReadValue();
        //}

        //public async Task<JogTime_Model> ReadValues_Cement()
        //{
        //    return await ReadValue();
        //}

        //public async Task<JogTime_Model> ReadValues_Water()
        //{
        //    return await ReadValue();
        //}

        //public async Task<JogTime_Model> ReadValues_Admix()
        //{
        //    return await ReadValue();
        //}

        //public async Task<JogTime_Model> ReadValues_Silica()
        //{
        //    return await ReadValue();
        //}
    }
}