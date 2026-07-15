using System.Windows;
using MQTTnet;

public class MqttPublishSerice
{
    public async Task<string> PublishDataMqttTopic(string TopicName, string JsonString)
    {
        try
        {
            var factory = new MqttClientFactory();
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASender")
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(TopicName)
                .WithPayload(JsonString)
                .Build();

            await client.PublishAsync(message);

            await client.DisconnectAsync();

            return null;
        }
        catch (System.Exception)
        {
            //Saravanan please write log  capture Error & TopicName & JsonString.
            throw;
        }
    }
}