using MQTTnet;
using System.Text;
using System.Xml.Linq;
using Scada_Demo.MQTT_Model;
using System.Text.Json;
using Sharp7;
using System.Windows;

namespace Scada_Demo.Services
{
    public class MaterialinAir
    {
        //public async Task<MaterialInAir_Model> ReadValues()
        //{
        //    var data = new MaterialInAir_Model();

        //    var factory = new MqttClientFactory();
        //    var client = factory.CreateMqttClient();

        //    var options = new MqttClientOptionsBuilder()
        //        .WithClientId("SCADAReceiver")
        //        .WithTcpServer("localhost", 1883)
        //        .Build();

        //    await client.ConnectAsync(options);

        //    client.ApplicationMessageReceivedAsync += e =>
        //    {
        //        if (e.ApplicationMessage.Topic == "materialinair/read")
        //        {
        //            var json = Encoding.UTF8.GetString(
        //                e.ApplicationMessage.Payload.FirstSpan);

        //            var result =
        //                JsonSerializer.Deserialize<MaterialInAir_Model>(json);

        //            if (result != null)
        //                data = result;
        //        }

        //        return Task.CompletedTask;
        //    };

        //    await client.SubscribeAsync(
        //        new MqttTopicFilterBuilder()
        //            .WithTopic("materialinair/read")
        //            .Build());

        //    await Task.Delay(3000);

        //    await client.DisconnectAsync();

        //    return data;
        //}

        public async Task<BatchSettingsModel> ReadValues()
        {
            //var data = new BatchSettingsModel();

            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();
            var tcs = new TaskCompletionSource<BatchSettingsModel>();
            var options = new MqttClientOptionsBuilder()
       .WithClientId("SCADAReceiver")
       .WithTcpServer("localhost", 1883)
       .Build();

            await client.ConnectAsync(options);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var json = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                var result = JsonSerializer.Deserialize<BatchSettingsModel>(json);

                if (result != null)
                    tcs.TrySetResult(result);

                return Task.CompletedTask;
            };

            await client.SubscribeAsync("batchsettings/read");

            var data = await tcs.Task;

            await client.DisconnectAsync();
           
            return data;
        }


        public async Task WriteAGG_92(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/92")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteAGG_94(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/94")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteAGG_96(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/96")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }

        public async Task WriteAGG_98(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/98")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }


        public async Task WriteCEM_100(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("cement/write/100")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }


        public async Task WriteWTR_386(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/386")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }


        public async Task WriteADM_108(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/108")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }



        public async Task WriteADM_110(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/110")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }


        public async Task WriteICE_410(string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("materialinair/write/410")
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }
    }
}