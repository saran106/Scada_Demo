using System.ComponentModel;

namespace Scada_Demo.MQTT_Model
{
    public class BinSetupItem : INotifyPropertyChanged
    {
        public string BinCode { get; set; }

        public string BinType { get; set; }

        public int BinNumber { get; set; }

        private string _materialName;

        public string MaterialName
        {
            get => _materialName;
            set
            {
                _materialName = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(MaterialName)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}