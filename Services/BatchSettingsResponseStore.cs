using Scada_Demo.MQTT_Model;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.Services
{
    public class BatchSettingsResponseStore
    {

        private BatchSettingsModel _current = new();

        public BatchSettingsModel Current => _current;   

        public event EventHandler? DataReceived;
        private readonly MaterialInAirViewModel _MaterialInAirViewModel;
        public BatchSettingsResponseStore(MaterialInAirViewModel materialInAirViewModel)
        {
            _MaterialInAirViewModel = materialInAirViewModel;
        }

        public void Update(BatchSettingsModel data) 
        {
            _current = data;

            _MaterialInAirViewModel.ResponseStore_DataReceived(data);

            DataReceived?.Invoke(this, EventArgs.Empty);
        }
    }
}