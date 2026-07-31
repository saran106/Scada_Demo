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
            //LoadRecipeNames();
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

        private void LoadRecipeNames(string recipeId)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT RecipeName
          FROM RecipeMaster
          WHERE RecipeID=@RecipeID
          ORDER BY RecipeName", con);

                da.SelectCommand.Parameters.AddWithValue("@RecipeID", recipeId);

                DataTable dt = new();
                da.Fill(dt);

                cmbRecipeName.ItemsSource = dt.DefaultView;
                cmbRecipeName.DisplayMemberPath = "RecipeName";
                cmbRecipeName.SelectedValuePath = "RecipeName";
                cmbRecipeName.SelectedIndex = -1;
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

            LoadRecipeNames(cmbRecipeId.SelectedValue.ToString());
        }
        private void cmbRecipeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRecipeId.SelectedValue == null ||
                cmbRecipeName.SelectedValue == null)
                return;

            LoadRecipe(
                cmbRecipeId.SelectedValue.ToString(),
                cmbRecipeName.SelectedValue.ToString());
        }

        private void LoadRecipe(string recipeId, string recipeName)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
        SELECT *
        FROM RecipeMaster
        WHERE RecipeID=@RecipeID
          AND RecipeName=@RecipeName", con);

                cmd.Parameters.AddWithValue("@RecipeID", recipeId);
                cmd.Parameters.AddWithValue("@RecipeName", recipeName);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
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

                dr.Close();
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

       
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
