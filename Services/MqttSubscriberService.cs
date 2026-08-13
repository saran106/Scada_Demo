using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Buffers;
using System.Text;
using System.Text.Json;

namespace Scada_Demo.Services
{
    public class MqttSubscriberService
    {
        private readonly IMqttClient _client;
        private readonly BatchSettingsStore _store;
        private readonly BatchSettingsResponseStore _responseStore;

        private bool _reconnecting = false;

        public MqttSubscriberService(
            BatchSettingsStore store,
            BatchSettingsResponseStore responseStore)
        {
            _store = store;
            _responseStore = responseStore;

            var factory = new MqttClientFactory();
            _client = factory.CreateMqttClient();
        }

        public async Task StartAsync()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("SCADASubscriber")
                .WithTcpServer("localhost", 1883)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                .Build();

            _client.ConnectedAsync += async e =>
            {
                Console.WriteLine("✅ Subscriber Connected");

                await SubscribeTopicsAsync();
            };

            _client.DisconnectedAsync += async e =>
            {
                Console.WriteLine("❌ Subscriber Disconnected");

                await ReconnectAsync(options);
            };

            _client.ApplicationMessageReceivedAsync += OnMessageReceived;

            await _client.ConnectAsync(options);
        }

        private async Task SubscribeTopicsAsync()
        {
            try
            {
                await _client.SubscribeAsync("Mimic_Screen/read");
                await _client.SubscribeAsync("batchsettings/response");

                Console.WriteLine("📡 Subscribed : Mimic_Screen/read");
                Console.WriteLine("📡 Subscribed : batchsettings/response");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"❌ Subscribe failed: {ex.Message}");
            }
        }

        private async Task ReconnectAsync(
            MqttClientOptions options)
        {
            if (_reconnecting)
                return;

            _reconnecting = true;

            try
            {
                while (!_client.IsConnected)
                {
                    try
                    {
                        Console.WriteLine(
                            "🔄 Trying MQTT reconnect...");

                        await Task.Delay(3000);

                        await _client.ConnectAsync(options);

                        Console.WriteLine(
                            "✅ MQTT Reconnected");

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"❌ Reconnect failed: {ex.Message}");
                    }
                }
            }
            finally
            {
                _reconnecting = false;
            }
        }

        private Task OnMessageReceived(
            MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var topic = e.ApplicationMessage.Topic;

                var json = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.ToArray());

                var data =
                    JsonSerializer.Deserialize<BatchSettingsModel>(
                        json);

                if (data == null)
                    return Task.CompletedTask;

                Console.WriteLine(
                    $"📩 MQTT Received: {topic}");

                if (topic == "Mimic_Screen/read")
                {
                    Console.WriteLine(
                        "✅ Running Values Updated");

                    _store.Update(data);
                }
                else if (topic == "batchsettings/response")
                {
                    Console.WriteLine(
                        "✅ Write Response Updated");

                    _responseStore.Update(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"❌ MQTT Receive Error: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            if (_client.IsConnected)
            {
                await _client.DisconnectAsync();
            }
        }
    }
}