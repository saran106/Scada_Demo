using System.Text;
using System.Text.Json;
using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Text;
using System.Text.Json;
using MQTTnet;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Services
{
    public class Discharge_Delay
    {
       
public async Task<DischargeDelay_Model> ReadValues()
    {
        var data = new DischargeDelay_Model();

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

            if (topic == "dischargedelay/read")
            {
                var json = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                var result =
                    JsonSerializer.Deserialize<DischargeDelay_Model>(json);

                if (result != null)
                {
                    data = result;
                }
            }

            return Task.CompletedTask;
        };

        await client.SubscribeAsync(
            new MqttTopicFilterBuilder()
                .WithTopic("dischargedelay/read")
                .Build());

        await Task.Delay(3000);

        await client.DisconnectAsync();

        return data;
    }

    public async Task WriteCement(string value)
        {
            await Publish("dischargedelay/write/74", value);
        }

        public async Task WriteWater(string value)
        {
            await Publish("dischargedelay/write/432", value);
        }

        public async Task WriteAdmix(string value)
        {
            await Publish("dischargedelay/write/248", value);
        }

        public async Task WriteSkip(string value)
        {
            await Publish("dischargedelay/write/50", value);
        }

        public async Task WritePumpCutOff(string value)
        {
            await Publish("dischargedelay/write/604", value);
        }

        private async Task Publish(string topic, string value)
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