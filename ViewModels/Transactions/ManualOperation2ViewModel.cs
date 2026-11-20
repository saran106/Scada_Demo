
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Scada_Demo.Services;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.ViewModels.Transactions
{
    public class ManualOperation2ViewModel
    {
        private readonly MqttPublishSerice _publisher;

        private const string ManualOperationTopic = "ManualOperation2";

        // ADMIXTURE
        public ICommand ADM1DosPressCommand { get; private set; } = null!;
        public ICommand ADM1DosReleaseCommand { get; private set; } = null!;

        public ICommand ADM2DosPressCommand { get; private set; } = null!;
        public ICommand ADM2DosReleaseCommand { get; private set; } = null!;

        public ICommand ADMDisPressCommand { get; private set; } = null!;
        public ICommand ADMDisReleaseCommand { get; private set; } = null!;

        // WATER
        public ICommand WaterDosPressCommand { get; private set; } = null!;
        public ICommand WaterDosReleaseCommand { get; private set; } = null!;

        public ICommand WaterDisPressCommand { get; private set; } = null!;
        public ICommand WaterDisReleaseCommand { get; private set; } = null!;

        // CEMENT
        public ICommand Cement1PressCommand { get; private set; } = null!;
        public ICommand Cement1ReleaseCommand { get; private set; } = null!;

        public ICommand Cement2PressCommand { get; private set; } = null!;
        public ICommand Cement2ReleaseCommand { get; private set; } = null!;

        public ICommand Cement3PressCommand { get; private set; } = null!;
        public ICommand Cement3ReleaseCommand { get; private set; } = null!;

        public ICommand CementDisPressCommand { get; private set; } = null!;
        public ICommand CementDisReleaseCommand { get; private set; } = null!;

        // ICE
        public ICommand IceDosPressCommand { get; private set; } = null!;
        public ICommand IceDosReleaseCommand { get; private set; } = null!;

        public ICommand IceDisPressCommand { get; private set; } = null!;
        public ICommand IceDisReleaseCommand { get; private set; } = null!;

        // SILICA
        public ICommand SilicaDosPressCommand { get; private set; } = null!;
        public ICommand SilicaDosReleaseCommand { get; private set; } = null!;

        public ICommand SilicaDisPressCommand { get; private set; } = null!;
        public ICommand SilicaDisReleaseCommand { get; private set; } = null!;

        // SKIP
        public ICommand SkipUpPressCommand { get; private set; } = null!;
        public ICommand SkipUpReleaseCommand { get; private set; } = null!;

        public ICommand SkipStopPressCommand { get; private set; } = null!;
        public ICommand SkipStopReleaseCommand { get; private set; } = null!;

        public ICommand SkipDownPressCommand { get; private set; } = null!;
        public ICommand SkipDownReleaseCommand { get; private set; } = null!;

        // MIXER
        public ICommand MixerOnPressCommand { get; private set; } = null!;
        public ICommand MixerOnReleaseCommand { get; private set; } = null!;

        public ICommand MixerOffPressCommand { get; private set; } = null!;
        public ICommand MixerOffReleaseCommand { get; private set; } = null!;

        public ICommand MixerGateOpenPressCommand { get; private set; } = null!;
        public ICommand MixerGateOpenReleaseCommand { get; private set; } = null!;

        public ICommand MixerGateClosePressCommand { get; private set; } = null!;
        public ICommand MixerGateCloseReleaseCommand { get; private set; } = null!;

        // OTHER
        public ICommand TolPressCommand { get; private set; } = null!;
        public ICommand TolReleaseCommand { get; private set; } = null!;

        public ICommand FaultResetPressCommand { get; private set; } = null!;
        public ICommand FaultResetReleaseCommand { get; private set; } = null!;

        public ICommand HooterPressCommand { get; private set; } = null!;
        public ICommand HooterReleaseCommand { get; private set; } = null!;

        // AUTO / MANUAL
        public ICommand AutoManualPressCommand { get; private set; } = null!;
        public ICommand AutoManualReleaseCommand { get; private set; } = null!;


        // CONSTRUCTOR
        public ManualOperation2ViewModel(MqttPublishSerice publisher)
        {
            Debug.WriteLine("ManualOperation2ViewModel CREATED");

            _publisher = publisher
                ?? throw new ArgumentNullException(nameof(publisher));

            // ADMIXTURE
            (ADM1DosPressCommand, ADM1DosReleaseCommand) =
                CreateCommands("ADM1_DOS");

            (ADM2DosPressCommand, ADM2DosReleaseCommand) =
                CreateCommands("ADM2_DOS");

            (ADMDisPressCommand, ADMDisReleaseCommand) =
                CreateCommands("ADM_DIS");

            // WATER
            (WaterDosPressCommand, WaterDosReleaseCommand) =
                CreateCommands("WATER_DOS");

            (WaterDisPressCommand, WaterDisReleaseCommand) =
                CreateCommands("WATER_DIS");

            // CEMENT
            (Cement1PressCommand, Cement1ReleaseCommand) =
                CreateCommands("CEMENT1");

            (Cement2PressCommand, Cement2ReleaseCommand) =
                CreateCommands("CEMENT2");

            (Cement3PressCommand, Cement3ReleaseCommand) =
                CreateCommands("CEMENT3");

            (CementDisPressCommand, CementDisReleaseCommand) =
                CreateCommands("CEMENT_DIS");

            // ICE
            (IceDosPressCommand, IceDosReleaseCommand) =
                CreateCommands("ICE_DOS");

            (IceDisPressCommand, IceDisReleaseCommand) =
                CreateCommands("ICE_DIS");

            // SILICA
            (SilicaDosPressCommand, SilicaDosReleaseCommand) =
                CreateCommands("SILICA_DOS");

            (SilicaDisPressCommand, SilicaDisReleaseCommand) =
                CreateCommands("SILICA_DIS");

            // SKIP
            (SkipUpPressCommand, SkipUpReleaseCommand) =
                CreateCommands("SKIP_UP");

            (SkipStopPressCommand, SkipStopReleaseCommand) =
                CreateCommands("SKIP_STOP");

            (SkipDownPressCommand, SkipDownReleaseCommand) =
                CreateCommands("SKIP_DOWN");

            // MIXER
            (MixerOnPressCommand, MixerOnReleaseCommand) =
                CreateCommands("MIXER_ON");

            (MixerOffPressCommand, MixerOffReleaseCommand) =
                CreateCommands("MIXER_OFF");

            (MixerGateOpenPressCommand, MixerGateOpenReleaseCommand) =
                CreateCommands("MIXER_GATE_OPEN");

            (MixerGateClosePressCommand, MixerGateCloseReleaseCommand) =
                CreateCommands("MIXER_GATE_CLOSE");

            // OTHER
            (TolPressCommand, TolReleaseCommand) =
                CreateCommands("TOL");

            (FaultResetPressCommand, FaultResetReleaseCommand) =
                CreateCommands("FAULT_RESET");

            (HooterPressCommand, HooterReleaseCommand) =
                CreateCommands("HOOTER");

            // AUTO / MANUAL
            (AutoManualPressCommand, AutoManualReleaseCommand) =
                CreateCommands("AUTO_MANUAL");

            Debug.WriteLine("ALL MANUAL OPERATION COMMANDS CREATED");
        }


        // CREATE PRESS AND RELEASE COMMANDS
        private (ICommand Press, ICommand Release) CreateCommands(
            string buttonName)
        {
            ICommand press = new AsyncRelayCommand(
                () => PublishManualOperation(buttonName, true));

            ICommand release = new AsyncRelayCommand(
                () => PublishManualOperation(buttonName, false));

            return (press, release);
        }


        // MQTT PUBLISH METHOD
        private async Task PublishManualOperation(
            string buttonName,
            bool value)
        {
            Debug.WriteLine(
                $"VIEWMODEL HIT: {buttonName}, Value: {value}");

            var payload = new
            {
                Type = "ManualOperation2",
                button = buttonName,
                value = value
            };

            string json = JsonSerializer.Serialize(payload);

            Debug.WriteLine($"MQTT PAYLOAD: {json}");

            try
            {
                await _publisher.PublishDataMqttTopic(
                    ManualOperationTopic,
                    json);

                Debug.WriteLine(
                    $"MQTT PUBLISHED SUCCESS: {buttonName}, Value: {value}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MQTT PUBLISH ERROR: {ex}");

                MessageBox.Show(
                    $"MQTT Publish Failed!\n" +
                    $"Button: {buttonName}\n" +
                    $"Value: {value}\n\n{ex.Message}",
                    "Manual Operation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // EXISTING MQTT RESPONSE METHOD
        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {
            // Keep your existing MQTT response handling here.
        }
    }


    // ASYNC RELAY COMMAND
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;

        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> execute)
        {
            _execute = execute
                ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting;
        }

        public async void Execute(object? parameter)
        {
            if (_isExecuting)
                return;

            try
            {
                _isExecuting = true;

                CanExecuteChanged?.Invoke(
                    this,
                    EventArgs.Empty);

                await _execute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ASYNC COMMAND ERROR: {ex}");

                MessageBox.Show(
                    ex.Message,
                    "Command Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _isExecuting = false;

                CanExecuteChanged?.Invoke(
                    this,
                    EventArgs.Empty);
            }
        }
    }
}
