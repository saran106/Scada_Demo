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

namespace Scada_Demo.Views
{
    /// <summary>
    /// Interaction logic for Maintenance.xaml
    /// </summary>
    public partial class Maintenance : Window
    {
        public Maintenance()
        {
            InitializeComponent();
            LoadMaintenance();
        }
        private void LoadMaintenance()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Maint_ID FROM MaintenanceMaster ORDER BY Maint_ID", con);

                DataTable dt = new();
                da.Fill(dt);

                cmbMaintID.ItemsSource = dt.DefaultView;
                cmbMaintID.DisplayMemberPath = "Maint_ID";
                cmbMaintID.SelectedValuePath = "Maint_ID";
                cmbMaintID.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbMaintID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMaintID.SelectedValue == null)
                return;

            LoadRecord(cmbMaintID.SelectedValue.ToString());
        }

        private void LoadRecord(string id)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT *
              FROM MaintenanceMaster
              WHERE Maint_ID=@Maint_ID", con);

                cmd.Parameters.AddWithValue("@Maint_ID", id);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtMaintDesc.Text = dr["Maint_Desc"].ToString();
                    txtDaywise.Text = dr["Daywise_Trig"].ToString();
                    txtMixerwise.Text = dr["Mixerwise_Trig"].ToString();
                    txtProdwise.Text = dr["Prodwise_Trig"].ToString();
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
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string id = cmbMaintID.Text.Trim();

                SqlCommand check = new SqlCommand(
                    "SELECT Maint_ID FROM MaintenanceMaster WHERE Maint_ID=@Maint_ID", con);

                check.Parameters.AddWithValue("@Maint_ID", id);

                object result = check.ExecuteScalar();

                SqlCommand cmd;

                if (result != null)
                {
                    cmd = new SqlCommand(
                    @"UPDATE MaintenanceMaster
              SET Maint_Desc=@Maint_Desc,
                  Daywise_Trig=@Daywise,
                  Mixerwise_Trig=@Mixerwise,
                  Prodwise_Trig=@Prodwise
              WHERE Maint_ID=@Maint_ID", con);
                }
                else
                {
                    cmd = new SqlCommand(
                    @"INSERT INTO MaintenanceMaster
            (
                Maint_ID,
                Maint_Desc,
                Daywise_Trig,
                Mixerwise_Trig,
                Prodwise_Trig
            )
            VALUES
            (
                @Maint_ID,
                @Maint_Desc,
                @Daywise,
                @Mixerwise,
                @Prodwise
            )", con);
                }

                cmd.Parameters.AddWithValue("@Maint_ID", id);
                cmd.Parameters.AddWithValue("@Maint_Desc", txtMaintDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@Daywise", Convert.ToInt32(txtDaywise.Text));
                cmd.Parameters.AddWithValue("@Mixerwise", Convert.ToDecimal(txtMixerwise.Text));
                cmd.Parameters.AddWithValue("@Prodwise", Convert.ToDecimal(txtProdwise.Text));

                cmd.ExecuteNonQuery();

                MessageBox.Show(result != null
                    ? "Maintenance Updated Successfully."
                    : "Maintenance Saved Successfully.");

                LoadMaintenance();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
