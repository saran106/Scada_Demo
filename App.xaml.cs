using System.Windows;
using Scada_Demo.Services;

namespace Scada_Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static BatchSettingsStore Store { get; private set; }

        public static MqttSubscriberService Subscriber { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Store = new BatchSettingsStore();
            Subscriber = new MqttSubscriberService(Store);

            try
            {
                await Subscriber.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"MQTT Server not available.\n\n{ex.Message}",
                    "MQTT",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (Subscriber != null)
            {
                await Subscriber.StopAsync();
            }

            base.OnExit(e);
        }
    }
}