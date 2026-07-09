using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Services
{
    public class BatchSettingsStore
    {
        public BatchSettingsModel? CurrentData { get; private set; }

        public event Action<BatchSettingsModel>? DataReceived;

        public void Update(BatchSettingsModel model)
        {
            CurrentData = model;
            DataReceived?.Invoke(model);
        }
    }
}