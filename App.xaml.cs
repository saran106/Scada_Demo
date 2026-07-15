using System.Windows;
using Scada_Demo.MQTT_Model;
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
        public static DischargeDelayViewModel DDVM { get; private set; }
        public static EmptyValueViewModel EmptyVM { get; private set; }
        public static GateSequenceViewModel GateSeqVM { get; private set; }
        public static StepTimeViewModel StepTimeVM { get; private set; }
        public static JogTimeViewModel JogTimeVM { get; private set; }
        public static ToleranceViewModel TolVM { get; private set; }
        public static MqttPublishSerice Publisher { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Publisher = new MqttPublishSerice();

            MaterialVM = new MaterialInAirViewModel(Publisher);
            DDVM = new DischargeDelayViewModel(Publisher);
            EmptyVM = new EmptyValueViewModel(Publisher);
            GateSeqVM = new GateSequenceViewModel(Publisher);
            StepTimeVM = new StepTimeViewModel(Publisher);
            JogTimeVM = new JogTimeViewModel(Publisher);
            TolVM = new ToleranceViewModel(Publisher);

            Store = new BatchSettingsStore();
            ResponseStore = new BatchSettingsResponseStore(MaterialVM, DDVM, EmptyVM, GateSeqVM, StepTimeVM,JogTimeVM,TolVM);


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