using MQTTnet;
using System.Text;
using System.Xml.Linq;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Services
{
    public class MaterialinAir
    {
        public async Task<MaterialInAir_Model> ReadValues_Agg()
        {
            var data = new MaterialInAir_Model();

            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADAReceiver")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;

                var value = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                switch (topic)
                {
                    case "materialinair/read/92":
                        data.Value92 = value;
                        break;

                    case "materialinair/read/94":
                        data.Value94 = value;
                        break;

                    case "materialinair/read/96":
                        data.Value96 = value;
                        break;

                    case "materialinair/read/98":
                        data.Value98 = value;
                        break;
                }

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("materialinair/read/#")
                    .Build());

            await Task.Delay(3000);

            await client.DisconnectAsync();

            return data;
        }


        public async Task<MaterialInAir_Model> ReadValues_WTR()
        {
            var data = new MaterialInAir_Model();

            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADAReceiver")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;

                var value = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                switch (topic)
                {
                    case "materialinair/read/386":
                        data.Wtr1 = value;
                        break;

                
                }

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("materialinair/read/#")
                    .Build());

            await Task.Delay(3000);

            await client.DisconnectAsync();

            return data;
        }

        public async Task<MaterialInAir_Model> ReadValues_Cement()
        {
            var data = new MaterialInAir_Model();

            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADAReceiver")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;

                var value = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                switch (topic)
                {
                    case "cement/read/100":
                        data.Cement1 = value;
                        break;

                    case "cement/read/102":
                        data.Cement2 = value;
                        data.Cement4 = value;
                        break;

                    case "cement/read/104":
                        data.Cement3 = value;
                        break;

                    //case "cement/read/102":
                    //    data.Cement4 = value;
                    //    break;
                }

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("cement/read/#")
                    .Build());

            await Task.Delay(3000);

            await client.DisconnectAsync();

            return data;
        }


        public async Task<MaterialInAir_Model> ReadValues_Admix()
        {
            var data = new MaterialInAir_Model();

            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADAReceiver")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;

                var value = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                switch (topic)
                {
                    case "materialinair/read/108":
                        data.Admix1 = value;
                        break;

                    case "materialinair/read/110":
                        data.Admix2 = value;
                       
                        break;

                
                        //case "cement/read/102":
                        //    data.Cement4 = value;
                        //    break;
                }

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("materialinair/read/#")
                    .Build());

            await Task.Delay(1000);

            await client.DisconnectAsync();

            return data;
        }


        public async Task<MaterialInAir_Model> ReadValues_ICE()
        {
            var data = new MaterialInAir_Model();

            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADAReceiver")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;

                var value = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                switch (topic)
                {
                    case "materialinair/read/410":
                        data.Silica = value;
                        break;

                 


                        //case "cement/read/102":
                        //    data.Cement4 = value;
                        //    break;
                }

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("materialinair/read/#")
                    .Build());

            await Task.Delay(1000);

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