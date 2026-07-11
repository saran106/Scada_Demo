using System.ComponentModel;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class MaterialInAirViewModel : INotifyPropertyChanged
    {

        private TaskCompletionSource<BatchSettingsModel>? _responseTcs;
        private string _agg92 = "";
        private string _agg94 = "";
        private string _agg96 = "";
        private string _agg98 = "";
        private string _water386 = "";
        private string _admix108 = "";
        private string _admix110 = "";
        private string _ice410 = "";

        private string _txtAgg1 = "";
        private string _txtAgg2 = "";
        private string _txtAgg3 = "";
        private string _txtAgg4 = "";

        private string _txtWTR1 = "";

        private string _txtAD1 = "";
        private string _txtAD2 = "";


        private string _txtICE = "";

        public string txtAgg1
        {
            get => _txtAgg1;
            set { _txtAgg1 = value; OnPropertyChanged(nameof(txtAgg1)); }
        } 
        
        public string txtAgg2
        {
            get => _txtAgg2;
            set { _txtAgg2 = value; OnPropertyChanged(nameof(txtAgg2)); }
        }

        public string txtAgg3
        {
            get => _txtAgg3;
            set { _txtAgg3 = value; OnPropertyChanged(nameof(txtAgg3)); }
        }

        public string txtAgg4
        {
            get => _txtAgg4;
            set { _txtAgg4 = value; OnPropertyChanged(nameof(txtAgg4)); }
        }
        
        public string txtWTR1
        {
            get => _txtWTR1;
            set { _txtWTR1 = value; OnPropertyChanged(nameof(txtWTR1)); }
        } 
        public string txtAD1
        {
            get => _txtAD1;
            set { _txtAD1 = value; OnPropertyChanged(nameof(txtAD1)); }
        }
        public string txtAD2
        {
            get => _txtAD2;
            set { _txtAD2 = value; OnPropertyChanged(nameof(txtAD2)); }
        }
        
        public string txtICE
        {
            get => _txtICE;
            set { _txtICE = value; OnPropertyChanged(nameof(txtICE)); }
        }
        public string Agg92
        {
            get => _agg92;
            set { _agg92 = value; OnPropertyChanged(nameof(Agg92)); }
        }

        public string Agg94
        {
            get => _agg94;
            set { _agg94 = value; OnPropertyChanged(nameof(Agg94)); }
        }

        public string Agg96
        {
            get => _agg96;
            set { _agg96 = value; OnPropertyChanged(nameof(Agg96)); }
        }

        public string Agg98
        {
            get => _agg98;
            set { _agg98 = value; OnPropertyChanged(nameof(Agg98)); }
        }

        public string Water386
        {
            get => _water386;
            set { _water386 = value; OnPropertyChanged(nameof(Water386)); }
        }

        public string Admix108
        {
            get => _admix108;
            set { _admix108 = value; OnPropertyChanged(nameof(Admix108)); }
        }

        public string Admix110
        {
            get => _admix110;
            set { _admix110 = value; OnPropertyChanged(nameof(Admix110)); }
        }

        public string Ice410
        {
            get => _ice410;
            set { _ice410 = value; OnPropertyChanged(nameof(Ice410)); }
        }

        public MaterialInAirViewModel()
        {
            //App.ResponseStore.DataReceived += ResponseStore_DataReceived;
        }

        public void ResponseStore_DataReceived(BatchSettingsModel data)
        {
            var mia = data.batchSettings_MaterialInAir;

            Agg92 = mia.Agg92.ToString();
            Agg94 = mia.Agg94.ToString();
            Agg96 = mia.Agg96.ToString();
            Agg98 = mia.Agg98.ToString();

            Water386 = mia.Water386.ToString();

            Admix108 = mia.Admix108.ToString();
            Admix110 = mia.Admix110.ToString();

            Ice410 = mia.Ice410.ToString();

            _responseTcs?.TrySetResult(data);

        }

        public async Task<bool> WriteToPLCAsync()
        {
            MaterialinAir mqtt = new MaterialinAir();

            BatchSettingsModel model = new BatchSettingsModel();

            model.Type = "MaterialInAir";

            model.batchSettings_MaterialInAir.Agg92 = Convert.ToInt16(txtAgg1);
            model.batchSettings_MaterialInAir.Agg94 = Convert.ToInt16(txtAgg2);
            model.batchSettings_MaterialInAir.Agg96 = Convert.ToInt16(txtAgg3);
            model.batchSettings_MaterialInAir.Agg98 = Convert.ToInt16(txtAgg4);

            model.batchSettings_MaterialInAir.Water386 = Convert.ToInt16(txtWTR1);

            model.batchSettings_MaterialInAir.Admix108 = Convert.ToInt16(txtAD1);
            model.batchSettings_MaterialInAir.Admix110 = Convert.ToInt16(txtAD2);

            model.batchSettings_MaterialInAir.Ice410 = Convert.ToInt16(txtICE);

            _responseTcs = new TaskCompletionSource<BatchSettingsModel>();

            await mqtt.WriteBatchSettings(model);

            var response = await _responseTcs.Task;

            return response.Type == "MaterialInAir"
                && response.batchSettings_MaterialInAir.Agg92 == model.batchSettings_MaterialInAir.Agg92
                && response.batchSettings_MaterialInAir.Agg94 == model.batchSettings_MaterialInAir.Agg94
                && response.batchSettings_MaterialInAir.Agg96 == model.batchSettings_MaterialInAir.Agg96
                && response.batchSettings_MaterialInAir.Agg98 == model.batchSettings_MaterialInAir.Agg98
                && response.batchSettings_MaterialInAir.Water386 == model.batchSettings_MaterialInAir.Water386
                && response.batchSettings_MaterialInAir.Admix108 == model.batchSettings_MaterialInAir.Admix108
                && response.batchSettings_MaterialInAir.Admix110 == model.batchSettings_MaterialInAir.Admix110
                && response.batchSettings_MaterialInAir.Ice410 == model.batchSettings_MaterialInAir.Ice410;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}