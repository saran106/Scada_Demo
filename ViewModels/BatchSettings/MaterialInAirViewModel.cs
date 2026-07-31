using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Scada_Demo.MQTT_Model;
using Scada_Demo.Services;
using Scada_Demo.Database;
using Scada_Demo.ViewModels.BatchSettings;

namespace Scada_Demo.ViewModels.BatchSettings
{
    public class MaterialInAirViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        private readonly MqttPublishSerice _mqttPublishSerice;
        public MaterialInAirViewModel(MqttPublishSerice mqttPublishSerice)
        {
            _mqttPublishSerice = mqttPublishSerice;

            // Aggregate
            ShowAggregateCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Aggregate";

                ShowOnly(
                    Visibility.Visible,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed);
            });

            // Cement
            ShowCementCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Cement";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Visible,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed);
            });

            // Water
            ShowWaterCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Water";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Visible,
                    Visibility.Collapsed,
                    Visibility.Collapsed);
            });

            // Admixture
            ShowAdmixCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Admixture";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Visible,
                    Visibility.Collapsed);
            });

            // Micro Silica
            ShowSilicaCommand = new RelayCommand(_ =>
            {
                SelectedTab = "Micro Silica";

                ShowOnly(
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Collapsed,
                    Visibility.Visible);
            });

            ReadFromPlcCommand = new RelayCommand(_ => ReadFromPlc());

            WriteToPlcCommand = new RelayCommand(_ => WriteToPlc());

            ExitCommand = new RelayCommand(_ => Exit());

            // Default Selected Tab
            SelectedTab = "Aggregate";

            LoadMaterialInAir();
        }

        #endregion

        private string _selectedTab = "Aggregate";

        public string SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        #region Aggregate (Agg1 - Agg6)

        private string _agg1;
        public string txtAgg1
        {
            get => _agg1;
            set => SetProperty(ref _agg1, value);
        }

        private string _agg2;
        public string txtAgg2
        {
            get => _agg2;
            set => SetProperty(ref _agg2, value);
        }

        private string _agg3;
        public string txtAgg3
        {
            get => _agg3;
            set => SetProperty(ref _agg3, value);
        }

        private string _agg4;
        public string txtAgg4
        {
            get => _agg4;
            set => SetProperty(ref _agg4, value);
        }

        private string _agg5;
        public string txtAgg5
        {
            get => _agg5;
            set => SetProperty(ref _agg5, value);
        }

        private string _agg6;
        public string txtAgg6
        {
            get => _agg6;
            set => SetProperty(ref _agg6, value);
        }

        #endregion

        #region Cement (Cem1 - Cem5)

        private string _c1;
        public string txtC1
        {
            get => _c1;
            set => SetProperty(ref _c1, value);
        }

        private string _c2;
        public string txtC2
        {
            get => _c2;
            set => SetProperty(ref _c2, value);
        }

        private string _c3;
        public string txtC3
        {
            get => _c3;
            set => SetProperty(ref _c3, value);
        }

        private string _c4;
        public string txtC4
        {
            get => _c4;
            set => SetProperty(ref _c4, value);
        }

        private string _c5;
        public string txtC5
        {
            get => _c5;
            set => SetProperty(ref _c5, value);
        }

        #endregion

        #region Water (Wtr1 - Wtr3)

        private string _wtr1;
        public string txtWTR1
        {
            get => _wtr1;
            set => SetProperty(ref _wtr1, value);
        }

        private string _wtr2;
        public string txtWTR2
        {
            get => _wtr2;
            set => SetProperty(ref _wtr2, value);
        }

        private string _wtr3;
        public string txtWTR3
        {
            get => _wtr3;
            set => SetProperty(ref _wtr3, value);
        }

        #endregion

        #region Admixture (Admix1 - Admix4)

        private string _ad1;
        public string txtAD1
        {
            get => _ad1;
            set => SetProperty(ref _ad1, value);
        }

        private string _ad2;
        public string txtAD2
        {
            get => _ad2;
            set => SetProperty(ref _ad2, value);
        }

        private string _ad3;
        public string txtAD3
        {
            get => _ad3;
            set => SetProperty(ref _ad3, value);
        }

        private string _ad4;
        public string txtAD4
        {
            get => _ad4;
            set => SetProperty(ref _ad4, value);
        }

        #endregion

        #region Micro Silica

        private string _ice;
        public string txtICE
        {
            get => _ice;
            set => SetProperty(ref _ice, value);
        }

        #endregion

        #region Tab / Panel switching

        // Bind each panel's Visibility to these instead of toggling in code-behind.
        // e.g. Visibility="{Binding AggregateVisibility}"
        private Visibility _aggregateVisibility = Visibility.Visible;
        public Visibility AggregateVisibility
        {
            get => _aggregateVisibility;
            set => SetProperty(ref _aggregateVisibility, value);
        }

        private Visibility _cementVisibility = Visibility.Collapsed;
        public Visibility CementVisibility
        {
            get => _cementVisibility;
            set => SetProperty(ref _cementVisibility, value);
        }

        private Visibility _waterVisibility = Visibility.Collapsed;
        public Visibility WaterVisibility
        {
            get => _waterVisibility;
            set => SetProperty(ref _waterVisibility, value);
        }

        private Visibility _admixVisibility = Visibility.Collapsed;
        public Visibility AdmixVisibility
        {
            get => _admixVisibility;
            set => SetProperty(ref _admixVisibility, value);
        }

        private Visibility _silicaVisibility = Visibility.Collapsed;
        public Visibility SilicaVisibility
        {
            get => _silicaVisibility;
            set => SetProperty(ref _silicaVisibility, value);
        }

        private void ShowOnly(Visibility agg, Visibility cem, Visibility wtr, Visibility adm, Visibility sil)
        {
            AggregateVisibility = agg;
            CementVisibility = cem;
            WaterVisibility = wtr;
            AdmixVisibility = adm;
            SilicaVisibility = sil;
        }

        #endregion

        #region Commands

        public ICommand ShowAggregateCommand { get; }
        public ICommand ShowCementCommand { get; }
        public ICommand ShowWaterCommand { get; }
        public ICommand ShowAdmixCommand { get; }
        public ICommand ShowSilicaCommand { get; }
        public ICommand ReadFromPlcCommand { get; }
        public ICommand WriteToPlcCommand { get; }
        public ICommand ExitCommand { get; }

        #endregion



        #region PLC actions - wire these up to your existing PLC/DB access layer

        private async Task ReadFromPlc()
        {
            try
            {
                var NewObj = new
                {
                    Type = "MIA",  //Rename what u want,
                    activity = "Read" //Read or Write
                };

                await _mqttPublishSerice.PublishDataMqttTopic("BatchSettings/Request", JsonSerializer.Serialize(NewObj));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task WriteToPlc()
        {
            try
            {
                var writeData = new MaterialInAir_Model();

                writeData.Agg92 = Convert.ToInt16(_agg1);
                writeData.Agg94 = Convert.ToInt16(_agg2);
                writeData.Agg96 = Convert.ToInt16(_agg3);
                writeData.Agg98 = Convert.ToInt16(_agg4);
                // add here remaing Tags.

                // Water
                writeData.Water386 = Convert.ToInt16(_wtr1);

                // Admixture
                writeData.Admix108 = Convert.ToInt16(_ad1);
                writeData.Admix110 = Convert.ToInt16(_ad2);

                // Micro Silica
                writeData.Ice410 = Convert.ToInt16(_ice);



                var NewObj = new
                {
                    Type = "MIA",  //Rename what u want,
                    activity = "Write",  //Read or Write
                    batchSettings_MaterialInAir = writeData
                };

                await _mqttPublishSerice.PublishDataMqttTopic("BatchSettings/Request", JsonSerializer.Serialize(NewObj));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exit()
        {
            Application.Current?.Windows[0]?.Close();
        }

        public void MqttReadSuccessStatus(BatchSettingsModel data)
        {
            try
            {

                //MessageBox.Show(Application.Current.Dispatcher.CheckAccess().ToString());
                if (data.Type == "MIA")
                {
                    if (data.activity == "Read")//Read Response.
                    {


                        //_agg1 = data.batchSettings_MaterialInAir.Agg92.ToString();
                        //_agg2 = data.batchSettings_MaterialInAir.Agg94.ToString();
                        //_agg3 = data.batchSettings_MaterialInAir.Agg96.ToString();
                        //_agg4 = data.batchSettings_MaterialInAir.Agg98.ToString();

                        txtAgg1 = data.batchSettings_MaterialInAir.Agg92.ToString();
                        txtAgg2 = data.batchSettings_MaterialInAir.Agg94.ToString();
                        txtAgg3 = data.batchSettings_MaterialInAir.Agg96.ToString();
                        txtAgg4 = data.batchSettings_MaterialInAir.Agg98.ToString();

                        txtWTR1 = data.batchSettings_MaterialInAir.Water386.ToString();

                        txtAD1 = data.batchSettings_MaterialInAir.Admix108.ToString();
                        txtAD2 = data.batchSettings_MaterialInAir.Admix110.ToString();

                        txtICE = data.batchSettings_MaterialInAir.Ice410.ToString();
                    }
                    else //Write Response.
                    {
                        //if(Convert.ToInt16(_agg1) == data.batchSettings_MaterialInAir.Agg92 &&
                        //    Convert.ToInt16(_agg2) == data.batchSettings_MaterialInAir.Agg94 &&
                        //    Convert.ToInt16(_agg3) == data.batchSettings_MaterialInAir.Agg96 &&
                        //    Convert.ToInt16(_agg4) == data.batchSettings_MaterialInAir.Agg98) 
                        if (
        Convert.ToInt16(_agg1) == data.batchSettings_MaterialInAir.Agg92 &&
        Convert.ToInt16(_agg2) == data.batchSettings_MaterialInAir.Agg94 &&
        Convert.ToInt16(_agg3) == data.batchSettings_MaterialInAir.Agg96 &&
        Convert.ToInt16(_agg4) == data.batchSettings_MaterialInAir.Agg98 &&

        Convert.ToInt16(_wtr1) == data.batchSettings_MaterialInAir.Water386 &&

        Convert.ToInt16(_ad1) == data.batchSettings_MaterialInAir.Admix108 &&
        Convert.ToInt16(_ad2) == data.batchSettings_MaterialInAir.Admix110 &&

        Convert.ToInt16(_ice) == data.batchSettings_MaterialInAir.Ice410
    )//Add Remaing Tags
                        {
                            //Write database table save activity here....   
                            SaveMaterialInAir();

                            MessageBox.Show("Data saved successfully.!");


                        }
                        else
                        {
                            MessageBox.Show("Failed.! Try again.!");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void SaveMaterialInAir()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM MaterialEmptyValue_Setup)
BEGIN
    UPDATE MaterialEmptyValue_Setup
    SET
        Gate1=@Gate1,
        Gate2=@Gate2,
        Gate3=@Gate3,
        Gate4=@Gate4,
        Gate5=@Gate5,
        Gate6=@Gate6,

        Cement1=@Cement1,
        Cement2=@Cement2,
        Cement3=@Cement3,
        Cement4=@Cement4,
        Cement5=@Cement5,

        Water=@Water,
        Water2=@Water2,
        Water3=@Water3,

        Admix1=@Admix1,
        Admix2=@Admix2,
        Admix3=@Admix3,
        Admix4=@Admix4,

        Silica=@Silica
END
ELSE
BEGIN
    INSERT INTO MaterialEmptyValue_Setup
    (
        Gate1,Gate2,Gate3,Gate4,Gate5,Gate6,
        Cement1,Cement2,Cement3,Cement4,Cement5,
        Water,Water2,Water3,
        Admix1,Admix2,Admix3,Admix4,
        Silica
    )
    VALUES
    (
        @Gate1,@Gate2,@Gate3,@Gate4,@Gate5,@Gate6,
        @Cement1,@Cement2,@Cement3,@Cement4,@Cement5,
        @Water,@Water2,@Water3,
        @Admix1,@Admix2,@Admix3,@Admix4,
        @Silica
    )
END", con);

                    // Aggregate
                    cmd.Parameters.AddWithValue("@Gate1", string.IsNullOrWhiteSpace(txtAgg1) ? (object)DBNull.Value : Convert.ToInt32(txtAgg1));
                    cmd.Parameters.AddWithValue("@Gate2", string.IsNullOrWhiteSpace(txtAgg2) ? (object)DBNull.Value : Convert.ToInt32(txtAgg2));
                    cmd.Parameters.AddWithValue("@Gate3", string.IsNullOrWhiteSpace(txtAgg3) ? (object)DBNull.Value : Convert.ToInt32(txtAgg3));
                    cmd.Parameters.AddWithValue("@Gate4", string.IsNullOrWhiteSpace(txtAgg4) ? (object)DBNull.Value : Convert.ToInt32(txtAgg4));
                    cmd.Parameters.AddWithValue("@Gate5", string.IsNullOrWhiteSpace(txtAgg5) ? (object)DBNull.Value : Convert.ToInt32(txtAgg5));
                    cmd.Parameters.AddWithValue("@Gate6", string.IsNullOrWhiteSpace(txtAgg6) ? (object)DBNull.Value : Convert.ToInt32(txtAgg6));

                    // Cement
                    cmd.Parameters.AddWithValue("@Cement1", string.IsNullOrWhiteSpace(txtC1) ? (object)DBNull.Value : Convert.ToInt32(txtC1));
                    cmd.Parameters.AddWithValue("@Cement2", string.IsNullOrWhiteSpace(txtC2) ? (object)DBNull.Value : Convert.ToInt32(txtC2));
                    cmd.Parameters.AddWithValue("@Cement3", string.IsNullOrWhiteSpace(txtC3) ? (object)DBNull.Value : Convert.ToInt32(txtC3));
                    cmd.Parameters.AddWithValue("@Cement4", string.IsNullOrWhiteSpace(txtC4) ? (object)DBNull.Value : Convert.ToInt32(txtC4));
                    cmd.Parameters.AddWithValue("@Cement5", string.IsNullOrWhiteSpace(txtC5) ? (object)DBNull.Value : Convert.ToInt32(txtC5));

                    // Water
                    cmd.Parameters.AddWithValue("@Water", string.IsNullOrWhiteSpace(txtWTR1) ? (object)DBNull.Value : Convert.ToInt32(txtWTR1));
                    cmd.Parameters.AddWithValue("@Water2", string.IsNullOrWhiteSpace(txtWTR2) ? (object)DBNull.Value : Convert.ToInt32(txtWTR2));
                    cmd.Parameters.AddWithValue("@Water3", string.IsNullOrWhiteSpace(txtWTR3) ? (object)DBNull.Value : Convert.ToInt32(txtWTR3));

                    // Admix
                    cmd.Parameters.AddWithValue("@Admix1", string.IsNullOrWhiteSpace(txtAD1) ? (object)DBNull.Value : Convert.ToDecimal(txtAD1));
                    cmd.Parameters.AddWithValue("@Admix2", string.IsNullOrWhiteSpace(txtAD2) ? (object)DBNull.Value : Convert.ToDecimal(txtAD2));
                    cmd.Parameters.AddWithValue("@Admix3", string.IsNullOrWhiteSpace(txtAD3) ? (object)DBNull.Value : Convert.ToDecimal(txtAD3));
                    cmd.Parameters.AddWithValue("@Admix4", string.IsNullOrWhiteSpace(txtAD4) ? (object)DBNull.Value : Convert.ToDecimal(txtAD4));

                    // Silica
                    cmd.Parameters.AddWithValue("@Silica", string.IsNullOrWhiteSpace(txtICE) ? (object)DBNull.Value : Convert.ToInt32(txtICE));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadMaterialInAir()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 * FROM MaterialEmptyValue_Setup", con);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtAgg1 = dr["Gate1"]?.ToString();
                        txtAgg2 = dr["Gate2"]?.ToString();
                        txtAgg3 = dr["Gate3"]?.ToString();
                        txtAgg4 = dr["Gate4"]?.ToString();
                        txtAgg5 = dr["Gate5"]?.ToString();
                        txtAgg6 = dr["Gate6"]?.ToString();

                        txtC1 = dr["Cement1"]?.ToString();
                        txtC2 = dr["Cement2"]?.ToString();
                        txtC3 = dr["Cement3"]?.ToString();
                        txtC4 = dr["Cement4"]?.ToString();
                        txtC5 = dr["Cement5"]?.ToString();

                        txtWTR1 = dr["Water"]?.ToString();
                        txtWTR2 = dr["Water2"]?.ToString();
                        txtWTR3 = dr["Water3"]?.ToString();

                        txtAD1 = dr["Admix1"]?.ToString();
                        txtAD2 = dr["Admix2"]?.ToString();
                        txtAD3 = dr["Admix3"]?.ToString();
                        txtAD4 = dr["Admix4"]?.ToString();

                        txtICE = dr["Silica"]?.ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}