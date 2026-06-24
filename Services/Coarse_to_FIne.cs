using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Services
{
    public class Coarse_to_FIne
    {
        //public async Task<Coarse_to_Fine> ReadValues_CF()
        //{
        //    var data = new Coarse_to_Fine();

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
        //            case "CF/read/538":
        //                data.Agg1 = value;
        //                break;

        //            case "CF/read/540":
        //                data.Agg2 = value;
        //                break;

        //            case "CF/read/542":
        //                data.Agg3 = value;
        //                break;

        //            case "CF/read/544":
        //                data.Agg4 = value;
        //                break;

        //            case "CF/read/546":
        //                data.Agg5 = value;
        //                break;

        //            case "CF/read/606":
        //                data.Wtr1 = value;
        //                break;
        //        }

        //        return Task.CompletedTask;
        //    };

        //    await client.SubscribeAsync(
        //        new MqttTopicFilterBuilder()
        //            .WithTopic("CF/read/#")
        //            .Build());

        //    await Task.Delay(3000);

        //    await client.DisconnectAsync();

        //    return data;
        //}

        public async Task WriteCF_538(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("CF/write/538")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }


        public async Task WriteCF_540(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("CF/write/540")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteCF_542(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("CF/write/542")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteCF_544(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("CF/write/544")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteCF_546(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("CF/write/546")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteCF_606(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("CF/write/606")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }
    }
}
