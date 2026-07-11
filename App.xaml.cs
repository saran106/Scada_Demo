using System.Windows;
using Scada_Demo.Services;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo
{
    public partial class App : Application
    {
        public static BatchSettingsStore Store { get; private set; }

        public static BatchSettingsResponseStore ResponseStore { get; private set; }

        public static MqttSubscriberService Subscriber { get; private set; }

        // Single ViewModel instance
        public static MaterialInAirViewModel MaterialVM { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MaterialVM = new MaterialInAirViewModel();

            Store = new BatchSettingsStore();
            ResponseStore = new BatchSettingsResponseStore(MaterialVM);
            Subscriber = new MqttSubscriberService(Store, ResponseStore);

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