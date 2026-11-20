using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using Scada_Demo.MQTT_Model;

namespace Scada_Demo.Views
{
    /// <summary>
    /// Interaction logic for Recipe_Details.xaml
    /// </summary>
    public partial class Recipe_Details : Window
    {
        public Recipe_Details()
        {
            InitializeComponent();

            LoadRecipeIds();
            LoadBinNames();
            ResetTabs();
            btnAggregate.Tag = "Active";
            AggregatePanel.Visibility = Visibility.Visible;
            App.Store.DataReceived += Store_BatchSettingsChanged;
        }

        private void Store_BatchSettingsChanged(BatchSettingsModel e)
        {
            Dispatcher.Invoke(() =>
            {
                txtAgg1.Text = e.batchSettings_Recipe.Agg1.ToString();
                txtAgg2.Text = e.batchSettings_Recipe.Agg2.ToString();
                txtAgg3.Text = e.batchSettings_Recipe.Agg3.ToString();
                txtAgg4.Text = e.batchSettings_Recipe.Agg4.ToString();

                txtCem1.Text = e.batchSettings_Recipe.Cem1.ToString();
                txtCem2.Text = e.batchSettings_Recipe.Cem2.ToString();

                txtAdm1.Text = e.batchSettings_Recipe.Adm1.ToString();
                txtAdm2.Text = e.batchSettings_Recipe.Adm2.ToString();

                txtWtr1.Text = e.batchSettings_Recipe.Water.ToString();

                txtSilica.Text = e.batchSettings_Recipe.IceSil.ToString();

                txtMixingTime.Text = e.batchSettings_Recipe.MixingTime.ToString();
                txtPreMixingTime.Text = e.batchSettings_Recipe.PremixTime.ToString();
            });
        }

        private void ResetTabs()
        {
            btnAggregate.Tag = null;
            btnCement.Tag = null;
            btnWater.Tag = null;
            btnAdmix.Tag = null;
            btnSilica.Tag = null;
        }

        private void ShowAggregate(object sender, RoutedEventArgs e)
        {
            HideAll();

            AggregatePanel.Visibility = Visibility.Visible;

            ResetTabs();
            btnAggregate.Tag = "Active";
        }


        private void ShowCement(object sender, RoutedEventArgs e)
        {
            HideAll();

            CementPanel.Visibility = Visibility.Visible;

            ResetTabs();
            btnCement.Tag = "Active";
        }

        private void ShowWater(object sender, RoutedEventArgs e)
        {
            HideAll();

            WaterPanel.Visibility = Visibility.Visible;

            ResetTabs();
            btnWater.Tag = "Active";
        }

        private void ShowAdmix(object sender, RoutedEventArgs e)
        {
            HideAll();

            AdmixPanel.Visibility = Visibility.Visible;

            ResetTabs();
            btnAdmix.Tag = "Active";
        }

        private void ShowSilica(object sender, RoutedEventArgs e)
        {
            HideAll();

            SilicaPanel.Visibility = Visibility.Visible;

            ResetTabs();
            btnSilica.Tag = "Active";
        }
        private void LoadRecipeIds()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT RecipeID FROM RecipeMaster ORDER BY RecipeID", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbRecipeId.ItemsSource = dt.DefaultView;
                    cmbRecipeId.DisplayMemberPath = "RecipeID";
                    cmbRecipeId.SelectedValuePath = "RecipeID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbRecipeId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRecipeId.SelectedValue == null)
                return;

            LoadRecipe(cmbRecipeId.SelectedValue.ToString());
        }

        private void LoadRecipe(string recipeId)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                using SqlCommand cmd = new SqlCommand(@"
                    SELECT *
                    FROM RecipeMaster
                    WHERE RecipeID = @RecipeID", con);

                cmd.Parameters.Add("@RecipeID", SqlDbType.VarChar, 50).Value = recipeId.Trim();

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Recipe Name is editable text, not part of the key.
                    txtRecipeName.Text = dr["RecipeName"]?.ToString();

                    // Left Panel
                    txtStrength.Text = dr["Strength"].ToString();
                    txtConsistency.Text = dr["Consistency"].ToString();
                    txtMixerCapacity.Text = dr["MixerCapacity"].ToString();
                    txtMixingTime.Text = dr["MixingTime"].ToString();
                    txtMixerDischarge.Text = dr["MixerDischargeTime"].ToString();
                    txtPreMixingTime.Text = dr["PreMixingTime"].ToString();
                    txtWaterCementRatio.Text = dr["WaterCementRatio"].ToString();
                    txtTotalMass.Text = dr["TotalMass"].ToString();

                    // Aggregate
                    txtAgg1.Text = dr["Aggregate1Weight"].ToString();
                    txtAgg2.Text = dr["Aggregate2Weight"].ToString();
                    txtAgg3.Text = dr["Aggregate3Weight"].ToString();
                    txtAgg4.Text = dr["Aggregate4Weight"].ToString();
                    txtAgg5.Text = dr["Aggregate5Weight"].ToString();
                    txtAgg6.Text = dr["Aggregate6Weight"].ToString();

                    // Cement
                    txtCem1.Text = dr["Cement1Weight"].ToString();
                    txtCem2.Text = dr["Cement2Weight"].ToString();
                    txtCem3.Text = dr["Cement3Weight"].ToString();
                    txtCem4.Text = dr["Cement4Weight"].ToString();
                    txtCem5.Text = dr["Cement5Weight"].ToString();

                    // Water
                    txtWtr1.Text = dr["Water1Weight"].ToString();
                    txtWtr2.Text = dr["Water2Weight"].ToString();
                    txtWtr3.Text = dr["Water3Weight"].ToString();

                    // Admixture
                    txtAdm1.Text = dr["Admixture1Weight"].ToString();
                    txtAdm2.Text = dr["Admixture2Weight"].ToString();
                    txtAdm3.Text = dr["Admixture3Weight"].ToString();
                    txtAdm4.Text = dr["Admixture4Weight"].ToString();

                    // Silica
                    txtSilica.Text = dr["SilicaWeight"].ToString();
                }
                else
                {
                    txtRecipeName.Clear();
                    ClearRecipeFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearRecipeFields()
        {
            txtStrength.Clear();
            txtConsistency.Clear();
            txtMixerCapacity.Clear();
            txtMixingTime.Clear();
            txtMixerDischarge.Clear();
            txtPreMixingTime.Clear();
            txtWaterCementRatio.Clear();
            txtTotalMass.Clear();

            txtAgg1.Clear();
            txtAgg2.Clear();
            txtAgg3.Clear();
            txtAgg4.Clear();
            txtAgg5.Clear();
            txtAgg6.Clear();

            txtCem1.Clear();
            txtCem2.Clear();
            txtCem3.Clear();
            txtCem4.Clear();
            txtCem5.Clear();

            txtWtr1.Clear();
            txtWtr2.Clear();
            txtWtr3.Clear();

            txtAdm1.Clear();
            txtAdm2.Clear();
            txtAdm3.Clear();
            txtAdm4.Clear();

            txtSilica.Clear();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbRecipeId.Text))
                {
                    MessageBox.Show("Please select a Recipe ID.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRecipeName.Text))
                {
                    MessageBox.Show("Please enter a Recipe Name.");
                    txtRecipeName.Focus();
                    return;
                }

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SaveRecipe(con);

                    MessageBox.Show("Recipe Saved Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private object GetDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;

            decimal result;
            if (decimal.TryParse(value, out result))
                return result;

            return DBNull.Value;
        }

        private object GetInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DBNull.Value;

            int result;
            if (int.TryParse(value, out result))
                return result;

            return DBNull.Value;
        }

        private void SaveRecipe(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SP_Recipe_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RecipeID", cmbRecipeId.Text.Trim());
            cmd.Parameters.AddWithValue("@RecipeName", txtRecipeName.Text.Trim());

            cmd.Parameters.AddWithValue("@Strength", txtStrength.Text.Trim());
            cmd.Parameters.AddWithValue("@Consistency", txtConsistency.Text.Trim());

            cmd.Parameters.AddWithValue("@MixerCapacity", GetDecimal(txtMixerCapacity.Text));
            cmd.Parameters.AddWithValue("@MixingTime", GetInt(txtMixingTime.Text));
            cmd.Parameters.AddWithValue("@MixerDischargeTime", GetInt(txtMixerDischarge.Text));
            cmd.Parameters.AddWithValue("@PreMixingTime", GetInt(txtPreMixingTime.Text));

            cmd.Parameters.AddWithValue("@WaterCementRatio", GetDecimal(txtWaterCementRatio.Text));
            cmd.Parameters.AddWithValue("@TotalMass", GetDecimal(txtTotalMass.Text));

            cmd.Parameters.AddWithValue("@Agg1", GetBinWeight(txtAgg1, "Aggregate1"));
            cmd.Parameters.AddWithValue("@Agg2", GetBinWeight(txtAgg2, "Aggregate2"));
            cmd.Parameters.AddWithValue("@Agg3", GetBinWeight(txtAgg3, "Aggregate3"));
            cmd.Parameters.AddWithValue("@Agg4", GetBinWeight(txtAgg4, "Aggregate4"));
            cmd.Parameters.AddWithValue("@Agg5", GetBinWeight(txtAgg5, "Aggregate5"));
            cmd.Parameters.AddWithValue("@Agg6", GetBinWeight(txtAgg6, "Aggregate6"));

            cmd.Parameters.AddWithValue("@Cem1", GetBinWeight(txtCem1, "Cement1"));
            cmd.Parameters.AddWithValue("@Cem2", GetBinWeight(txtCem2, "Cement2"));
            cmd.Parameters.AddWithValue("@Cem3", GetBinWeight(txtCem3, "Cement3"));
            cmd.Parameters.AddWithValue("@Cem4", GetBinWeight(txtCem4, "Cement4"));
            cmd.Parameters.AddWithValue("@Cem5", GetBinWeight(txtCem5, "Cement5"));

            cmd.Parameters.AddWithValue("@Wat1", GetBinWeight(txtWtr1, "Water1"));
            cmd.Parameters.AddWithValue("@Wat2", GetBinWeight(txtWtr2, "Water2"));
            cmd.Parameters.AddWithValue("@Wat3", GetBinWeight(txtWtr3, "Water3"));

            cmd.Parameters.AddWithValue("@Adm1", GetBinWeight(txtAdm1, "Admixture1"));
            cmd.Parameters.AddWithValue("@Adm2", GetBinWeight(txtAdm2, "Admixture2"));
            cmd.Parameters.AddWithValue("@Adm3", GetBinWeight(txtAdm3, "Admixture3"));
            cmd.Parameters.AddWithValue("@Adm4", GetBinWeight(txtAdm4, "Admixture4"));

            cmd.Parameters.AddWithValue("@Silica", GetBinWeight(txtSilica, "Silica1"));


            // Aggregate BinCodes
            cmd.Parameters.AddWithValue("@Aggregate1Name", "Aggregate1");
            cmd.Parameters.AddWithValue("@Aggregate2Name", "Aggregate2");
            cmd.Parameters.AddWithValue("@Aggregate3Name", "Aggregate3");
            cmd.Parameters.AddWithValue("@Aggregate4Name", "Aggregate4");
            cmd.Parameters.AddWithValue("@Aggregate5Name", "Aggregate5");
            cmd.Parameters.AddWithValue("@Aggregate6Name", "Aggregate6");

            // Cement BinCodes
            cmd.Parameters.AddWithValue("@Cement1Name", "Cement1");
            cmd.Parameters.AddWithValue("@Cement2Name", "Cement2");
            cmd.Parameters.AddWithValue("@Cement3Name", "Cement3");
            cmd.Parameters.AddWithValue("@Cement4Name", "Cement4");
            cmd.Parameters.AddWithValue("@Cement5Name", "Cement5");

            // Water BinCodes
            cmd.Parameters.AddWithValue("@Water1Name", "Water1");
            cmd.Parameters.AddWithValue("@Water2Name", "Water2");
            cmd.Parameters.AddWithValue("@Water3Name", "Water3");

            // Admixture BinCodes
            cmd.Parameters.AddWithValue("@Admixture1Name", "Admixture1");
            cmd.Parameters.AddWithValue("@Admixture2Name", "Admixture2");
            cmd.Parameters.AddWithValue("@Admixture3Name", "Admixture3");
            cmd.Parameters.AddWithValue("@Admixture4Name", "Admixture4");

            // Silica BinCode
            cmd.Parameters.AddWithValue("@SilicaName", "Silica1");

            cmd.ExecuteNonQuery();
        }

        private void HideAll()
        {
            AggregatePanel.Visibility = Visibility.Collapsed;
            CementPanel.Visibility = Visibility.Collapsed;
            WaterPanel.Visibility = Visibility.Collapsed;
            AdmixPanel.Visibility = Visibility.Collapsed;
            SilicaPanel.Visibility = Visibility.Collapsed;
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private Dictionary<string, string> binNames =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private HashSet<string> configuredBins =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private void LoadBinNames()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                using SqlCommand cmd = new SqlCommand("SP_BinSetup_GetNames", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader dr = cmd.ExecuteReader();

                binNames.Clear();
                configuredBins.Clear();

                while (dr.Read())
                {
                    string code = dr["BinCode"]?.ToString()?.Trim();

                    string materialName = dr["MaterialName"] == DBNull.Value
                        ? null
                        : dr["MaterialName"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(code) &&
                        !string.IsNullOrWhiteSpace(materialName))
                    {
                        binNames[code] = materialName;
                        configuredBins.Add(code);
                    }
                }

                // Aggregate
                SetBinVisibility("Aggregate1", lblAgg1, txtAgg1);
                SetBinVisibility("Aggregate2", lblAgg2, txtAgg2);
                SetBinVisibility("Aggregate3", lblAgg3, txtAgg3);
                SetBinVisibility("Aggregate4", lblAgg4, txtAgg4);
                SetBinVisibility("Aggregate5", lblAgg5, txtAgg5);
                SetBinVisibility("Aggregate6", lblAgg6, txtAgg6);

                // Cement
                SetBinVisibility("Cement1", lblCem1, txtCem1);
                SetBinVisibility("Cement2", lblCem2, txtCem2);
                SetBinVisibility("Cement3", lblCem3, txtCem3);
                SetBinVisibility("Cement4", lblCem4, txtCem4);
                SetBinVisibility("Cement5", lblCem5, txtCem5);

                // Water
                SetBinVisibility("Water1", lblWat1, txtWtr1);
                SetBinVisibility("Water2", lblWat2, txtWtr2);
                SetBinVisibility("Water3", lblWat3, txtWtr3);

                // Admixture
                SetBinVisibility("Admixture1", lblAdm1, txtAdm1);
                SetBinVisibility("Admixture2", lblAdm2, txtAdm2);
                SetBinVisibility("Admixture3", lblAdm3, txtAdm3);
                SetBinVisibility("Admixture4", lblAdm4, txtAdm4);

                // Silica
                bool silicaConfigured = configuredBins.Contains("Silica1");
                lblSilica.Text = GetBinName("Silica1");
                lblSilica.Visibility = silicaConfigured ? Visibility.Visible : Visibility.Collapsed;
                txtSilica.Visibility = silicaConfigured ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load Bin Names!\\n" + ex.Message);
            }
        }

        private void SetBinVisibility(string binCode, TextBlock label, TextBox textBox)
        {
            bool isConfigured = configuredBins.Contains(binCode);

            label.Text = GetBinName(binCode);

            // Collapse the complete row, not only its controls.
            // This removes empty row space for unconfigured bins.
            if (label.Parent is Grid rowGrid)
                rowGrid.Visibility = isConfigured ? Visibility.Visible : Visibility.Collapsed;

            label.Visibility = isConfigured ? Visibility.Visible : Visibility.Collapsed;
            textBox.Visibility = isConfigured ? Visibility.Visible : Visibility.Collapsed;
        }

        private decimal GetBinWeight(TextBox textBox, string binCode)
        {
            // Unconfigured bins must never retain an old recipe weight.
            if (!configuredBins.Contains(binCode))
                return 0m;

            decimal.TryParse(textBox.Text, out decimal value);
            return value;
        }

        private string GetBinName(string binCode)
        {
            return binNames.TryGetValue(binCode, out string name)
                ? name
                : binCode;
        }
    }
}
