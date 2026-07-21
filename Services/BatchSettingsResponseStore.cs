using Scada_Demo.MQTT_Model;
using System.Windows;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.Services
{
    public class BatchSettingsResponseStore
    {

        private BatchSettingsModel _current = new();

        public BatchSettingsModel Current => _current;

        public event EventHandler? DataReceived;
        private readonly MaterialInAirViewModel _MaterialInAirViewModel;
        private readonly DischargeDelayViewModel _DischargeDelayViewModel;
        private readonly EmptyValueViewModel _EmptyValueViewModel;
        private readonly GateSequenceViewModel _GateSequenceViewModel;
        private readonly StepTimeViewModel _StepTimeViewModel;
        private readonly JogTimeViewModel _JogTimeViewModel;
        private readonly ToleranceViewModel _ToleranceViewModel;
        private readonly CoarseToFineViewModel _CoarseToFineViewModel;
        public BatchSettingsResponseStore(MaterialInAirViewModel materialInAirViewModel, DischargeDelayViewModel DischargeDelayViewModel
            , EmptyValueViewModel emptyValueViewModel, GateSequenceViewModel gateSequenceViewModel, StepTimeViewModel stepTimeViewModel, JogTimeViewModel jogTimeViewModel, ToleranceViewModel toleranceViewModel
            , CoarseToFineViewModel coarsetofinemodel)
        {
            _MaterialInAirViewModel = materialInAirViewModel;
            _DischargeDelayViewModel = DischargeDelayViewModel;
            _EmptyValueViewModel = emptyValueViewModel;
            _GateSequenceViewModel = gateSequenceViewModel;
            _StepTimeViewModel = stepTimeViewModel;
            _JogTimeViewModel = jogTimeViewModel;
            _ToleranceViewModel = toleranceViewModel;
            _CoarseToFineViewModel = coarsetofinemodel;
        }

        public void Update(BatchSettingsModel data)
        {
            _current = data;

            switch (data.Type)
            {
                case "MIA":
                    {
                        _MaterialInAirViewModel.MqttReadSuccessStatus(data);
                        break;
                    }
                case "DischargeDelay":
                    {
                        _DischargeDelayViewModel.MqttReadSuccessStatus(data);
                        break;
                    }

                case "EmptyValue":
                    {
                        _EmptyValueViewModel.MqttReadSuccessStatus(data);
                        break;
                    }

                case "GateSequence":
                    {
                        _GateSequenceViewModel.MqttReadSuccessStatus(data);
                        break;
                    }

                case "StepTime":
                    {
                        _StepTimeViewModel.MqttReadSuccessStatus(data);
                        break;
                    } 

                case "JogTime":
                    {
                        _JogTimeViewModel.MqttReadSuccessStatus(data);
                        break;
                    } 
                case "Tolerance":
                    {
                        _ToleranceViewModel.MqttReadSuccessStatus(data);
                        break;
                    }

                case "CTF":
                    {
                        _CoarseToFineViewModel.MqttReadSuccessStatus(data);
                        break;
                    }



            }

            DataReceived?.Invoke(this, EventArgs.Empty);
            // });
            DataReceived?.Invoke(this, EventArgs.Empty);
        }
    }
}