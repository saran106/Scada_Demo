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
using System.Data;
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
            LoadRecipeNames();

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

        private void LoadRecipeNames()
        {
            try
            {
                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT RecipeID,RecipeName FROM RecipeMaster ORDER BY RecipeName", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbRecipeName.ItemsSource = dt.DefaultView;
                    cmbRecipeName.DisplayMemberPath = "RecipeName";
                    cmbRecipeName.SelectedValuePath = "RecipeID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
            cmd.Parameters.AddWithValue("@RecipeName", cmbRecipeName.Text.Trim());

            cmd.Parameters.AddWithValue("@Strength", txtStrength.Text.Trim());
            cmd.Parameters.AddWithValue("@Consistency", txtConsistency.Text.Trim());

            cmd.Parameters.AddWithValue("@MixerCapacity", GetDecimal(txtMixerCapacity.Text));
            cmd.Parameters.AddWithValue("@MixingTime", GetInt(txtMixingTime.Text));
            cmd.Parameters.AddWithValue("@MixerDischargeTime", GetInt(txtMixerDischarge.Text));
            cmd.Parameters.AddWithValue("@PreMixingTime", GetInt(txtPreMixingTime.Text));

            cmd.Parameters.AddWithValue("@WaterCementRatio", GetDecimal(txtWaterCementRatio.Text));
            cmd.Parameters.AddWithValue("@TotalMass", GetDecimal(txtTotalMass.Text));

            cmd.Parameters.AddWithValue("@Agg1", GetDecimal(txtAgg1.Text));
            cmd.Parameters.AddWithValue("@Agg2", GetDecimal(txtAgg2.Text));
            cmd.Parameters.AddWithValue("@Agg3", GetDecimal(txtAgg3.Text));
            cmd.Parameters.AddWithValue("@Agg4", GetDecimal(txtAgg4.Text));
            cmd.Parameters.AddWithValue("@Agg5", GetDecimal(txtAgg5.Text));
            cmd.Parameters.AddWithValue("@Agg6", GetDecimal(txtAgg6.Text));

            cmd.Parameters.AddWithValue("@Cem1", GetDecimal(txtCem1.Text));
            cmd.Parameters.AddWithValue("@Cem2", GetDecimal(txtCem2.Text));
            cmd.Parameters.AddWithValue("@Cem3", GetDecimal(txtCem3.Text));
            cmd.Parameters.AddWithValue("@Cem4", GetDecimal(txtCem4.Text));
            cmd.Parameters.AddWithValue("@Cem5", GetDecimal(txtCem5.Text));

            cmd.Parameters.AddWithValue("@Wat1", GetDecimal(txtWtr1.Text));
            cmd.Parameters.AddWithValue("@Wat2", GetDecimal(txtWtr2.Text));
            cmd.Parameters.AddWithValue("@Wat3", GetDecimal(txtWtr3.Text));

            cmd.Parameters.AddWithValue("@Adm1", GetDecimal(txtAdm1.Text));
            cmd.Parameters.AddWithValue("@Adm2", GetDecimal(txtAdm2.Text));
            cmd.Parameters.AddWithValue("@Adm3", GetDecimal(txtAdm3.Text));
            cmd.Parameters.AddWithValue("@Adm4", GetDecimal(txtAdm4.Text));

            cmd.Parameters.AddWithValue("@Silica", GetDecimal(txtSilica.Text));

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

        private void ShowAggregate(object sender, RoutedEventArgs e)
        {
            HideAll();
            AggregatePanel.Visibility = Visibility.Visible;
        }

        private void ShowCement(object sender, RoutedEventArgs e)
        {
            HideAll();
            CementPanel.Visibility = Visibility.Visible;
        }

        private void ShowWater(object sender, RoutedEventArgs e)
        {
            HideAll();
            WaterPanel.Visibility = Visibility.Visible;
        }

        private void ShowAdmix(object sender, RoutedEventArgs e)
        {
            HideAll();
            AdmixPanel.Visibility = Visibility.Visible;
        }

        private void ShowSilica(object sender, RoutedEventArgs e)
        {
            HideAll();
            SilicaPanel.Visibility = Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbRecipeId.Text))
                {
                    MessageBox.Show("Select Recipe ID");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this recipe?",
                                    "Confirm",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.No)
                    return;

                using (SqlConnection con = DbConnection.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM RecipeMaster WHERE RecipeID=@RecipeID", con);

                    cmd.Parameters.AddWithValue("@RecipeID", cmbRecipeId.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Recipe Deleted Successfully");

                LoadRecipeIds();
                LoadRecipeNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
