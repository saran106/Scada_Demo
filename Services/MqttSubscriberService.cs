//using MQTTnet;
//using Scada_Demo.MQTT_Model;
//using System.Text;
//using System.Text.Json;

//namespace Scada_Demo.Services
//{
//    public class MqttSubscriberService
//    {
//        private readonly IMqttClient _client;
//        private readonly BatchSettingsStore _store;

//        public MqttSubscriberService(BatchSettingsStore store)
//        {
//            _store = store;

//            var factory = new MqttClientFactory();
//            _client = factory.CreateMqttClient();
//        }

//        public async Task StartAsync()
//        {
//            var options = new MqttClientOptionsBuilder()
//              .WithClientId("SCADASubscriber")
//                .WithTcpServer("localhost", 1883)
//                .Build();

//            _client.ConnectedAsync += e =>
//            {
//                Console.WriteLine("✅ Subscriber Connected");
//                return Task.CompletedTask;
//            };

//            _client.DisconnectedAsync += e =>
//            {
//                Console.WriteLine("❌ Subscriber Disconnected");
//                return Task.CompletedTask;
//            };

//            _client.ApplicationMessageReceivedAsync += OnMessageReceived;

//            await _client.ConnectAsync(options);

//            await _client.SubscribeAsync("batchsettings/read");

//            Console.WriteLine("📡 Subscribed : batchsettings/read");
//        }

//        private Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
//        {
//            try
//            {
//                Console.WriteLine($"📩 Message Received : {e.ApplicationMessage.Topic}");

//                var json = Encoding.UTF8.GetString(
//                    e.ApplicationMessage.Payload.FirstSpan);

//                Console.WriteLine(json);

//                var data = JsonSerializer.Deserialize<BatchSettingsModel>(json);

//                if (data != null)
//                {
//                    Console.WriteLine("✅ Store Updated");

//                    _store.Update(data);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }

//            return Task.CompletedTask;
//        }

//        public async Task StopAsync()
//        {
//            if (_client.IsConnected)
//            {
//                await _client.DisconnectAsync();
//            }
//        }
//    }
//}

using MQTTnet;
using Scada_Demo.MQTT_Model;
using System.Text;
using System.Text.Json;

namespace Scada_Demo.Services
{
    public class MqttSubscriberService
    {
        private readonly IMqttClient _client;
        private readonly BatchSettingsStore _store;
        private readonly BatchSettingsResponseStore _responseStore;

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
                .Build();

            _client.ConnectedAsync += e =>
            {
                Console.WriteLine("✅ Subscriber Connected");
                return Task.CompletedTask;
            };

            _client.DisconnectedAsync += e =>
            {
                Console.WriteLine("❌ Subscriber Disconnected");
                return Task.CompletedTask;
            };

            _client.ApplicationMessageReceivedAsync += OnMessageReceived;

            await _client.ConnectAsync(options);

            await _client.SubscribeAsync("batchsettings/read");
            await _client.SubscribeAsync("batchsettingstest/response");

            Console.WriteLine("📡 Subscribed : batchsettings/read");
            Console.WriteLine("📡 Subscribed : batchsettingstest/response");
        }

        private Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var topic = e.ApplicationMessage.Topic;

                var json = Encoding.UTF8.GetString(
                    e.ApplicationMessage.Payload.FirstSpan);

                var data = JsonSerializer.Deserialize<BatchSettingsModel>(json);

                if (data == null)
                    return Task.CompletedTask;

                if (topic == "batchsettings/read")
                {
                    Console.WriteLine("✅ Running Values Updated");
                    _store.Update(data);
                }
                else if (topic == "batchsettingstest/response")
                {
                    Console.WriteLine("✅ Write Response Updated");
                    _responseStore.Update(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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