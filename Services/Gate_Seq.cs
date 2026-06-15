using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Text;

namespace Scada_Demo.Services
{
    public class Gate_Seq
    {
        public async Task<GateSeq_Model> ReadValues_GateSeq()
        {
            var data = new GateSeq_Model();

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
                    case "gateseq/read/210":
                        data.Agg1 = value;
                        break;

                    case "gateseq/read/212":
                        data.Agg3 = value;
                        break;

                    case "gateseq/read/214":
                        data.Agg4 = value;
                        break;
                }

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic("gateseq/read/#")
                    .Build());

            await Task.Delay(3000);

            await client.DisconnectAsync();

            return data;
        }

        public async Task WriteGATESEQ_210(string value)
        {
            await PublishValue("gateseq/write/210", value);
        }

        public async Task WriteGATESEQ_212(string value)
        {
            await PublishValue("gateseq/write/212", value);
        }

        public async Task WriteGATESEQ_214(string value)
        {
            await PublishValue("gateseq/write/214", value);
        }

        private async Task PublishValue(string topic, string value)
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(value)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();
        }
    }
}