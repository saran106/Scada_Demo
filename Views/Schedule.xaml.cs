
using Microsoft.Data.SqlClient;
using Scada_Demo.Database;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Scada_Demo.Views
{
    public partial class Schedule : Window
    {
        private bool _isLoadingSchedule = false;

        public Schedule()
        {
            InitializeComponent();

            LoadScheduleIDs();
            LoadAvailableOrders();
        }

        // =====================================
        // LOAD SCHEDULE IDS
        // =====================================

        private void LoadScheduleIDs()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT Sch_ID
                      FROM ScheduleMaster
                      ORDER BY Sch_ID", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbScheduleID.ItemsSource = dt.DefaultView;
                cmbScheduleID.DisplayMemberPath = "Sch_ID";
                cmbScheduleID.SelectedValuePath = "Sch_ID";
                cmbScheduleID.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // =====================================
        // SCHEDULE SELECTION
        // =====================================

        private void cmbScheduleID_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (_isLoadingSchedule)
                return;

            if (cmbScheduleID.SelectedValue == null)
                return;

            LoadSchedule(cmbScheduleID.SelectedValue.ToString());
        }

        // =====================================
        // LOAD EXISTING SCHEDULE
        // =====================================

        private void LoadSchedule(string schId)
        {
            try
            {
                _isLoadingSchedule = true;

                lstSelectedOrders.Items.Clear();

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                using SqlCommand cmd = new SqlCommand(
                    @"SELECT *
                      FROM ScheduleMaster
                      WHERE Sch_ID = @Sch_ID", con);

                cmd.Parameters.AddWithValue("@Sch_ID", schId);

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    dpScheduleDate.SelectedDate =
                        Convert.ToDateTime(dr["Sch_Date"]);

                    cmbScheduleType.Text =
                        dr["Sch_Type"].ToString();

                    txtTime.Text =
                        dr["Sch_Start_Time"] == DBNull.Value
                        ? ""
                        : Convert.ToDateTime(
                            dr["Sch_Start_Time"]).ToString("HH:mm");

                    // Comma-separated Order IDs
                    string orderIds = dr["Order_ID"].ToString();

                    foreach (string id in orderIds.Split(','))
                    {
                        string orderId = id.Trim();

                        if (!string.IsNullOrWhiteSpace(orderId))
                        {
                            lstSelectedOrders.Items.Add(orderId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Schedule Failed: " + ex.Message);
            }
            finally
            {
                _isLoadingSchedule = false;
            }
        }

        // =====================================
        // AVAILABLE ORDERS
        // =====================================

        private void LoadAvailableOrders()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT Order_ID
                      FROM OrderMaster
                      ORDER BY Order_ID", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                lstAvailableOrders.ItemsSource = dt.DefaultView;
                lstAvailableOrders.DisplayMemberPath = "Order_ID";
                lstAvailableOrders.SelectedValuePath = "Order_ID";
                lstAvailableOrders.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // =====================================
        // ADD ORDER
        // =====================================

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lstAvailableOrders.SelectedItem == null)
                return;

            DataRowView row =
                (DataRowView)lstAvailableOrders.SelectedItem;

            string orderId = row["Order_ID"].ToString().Trim();

            // FRONTEND DUPLICATE CHECK

            bool alreadySelected =
                lstSelectedOrders.Items
                .Cast<object>()
                .Any(x => string.Equals(
                    x.ToString(),
                    orderId,
                    StringComparison.OrdinalIgnoreCase));

            if (alreadySelected)
            {
                MessageBox.Show("Order already selected.");
                return;
            }

            lstSelectedOrders.Items.Add(orderId);

            lstAvailableOrders.SelectedIndex = -1;
        }

        // =====================================
        // REMOVE ORDER
        // =====================================

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstSelectedOrders.SelectedItem == null)
                return;

            lstSelectedOrders.Items.Remove(
                lstSelectedOrders.SelectedItem);
        }

        // =====================================
        // SAVE / UPDATE
        // =====================================

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string schId = cmbScheduleID.Text.Trim();

                if (string.IsNullOrWhiteSpace(schId))
                {
                    MessageBox.Show("Enter Schedule ID.");
                    return;
                }

                if (dpScheduleDate.SelectedDate == null)
                {
                    MessageBox.Show("Select Schedule Date.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbScheduleType.Text))
                {
                    MessageBox.Show("Select Schedule Type.");
                    return;
                }

                if (lstSelectedOrders.Items.Count == 0)
                {
                    MessageBox.Show("Select at least one Order.");
                    return;
                }

                // Final frontend duplicate validation

                List<string> orderList =
                    lstSelectedOrders.Items
                    .Cast<object>()
                    .Select(x => x.ToString().Trim())
                    .ToList();

                if (orderList.Distinct(
                    StringComparer.OrdinalIgnoreCase).Count()
                    != orderList.Count)
                {
                    MessageBox.Show("Duplicate Order IDs are not allowed.");
                    return;
                }

                // COMMA SEPARATED VALUES

                string orderIds = string.Join(",", orderList);

                // Time validation

                DateTime? startTime = null;

                if (!string.IsNullOrWhiteSpace(txtTime.Text))
                {
                    if (!TimeSpan.TryParse(
                        txtTime.Text.Trim(), out TimeSpan time))
                    {
                        MessageBox.Show(
                            "Enter valid time (Example: 08:30)");

                        txtTime.Focus();
                        return;
                    }

                    startTime = DateTime.Today.Add(time);
                }

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                // CHECK SCH_ID

                using SqlCommand check = new SqlCommand(
                    @"SELECT COUNT(*)
                      FROM ScheduleMaster
                      WHERE Sch_ID = @Sch_ID", con);

                check.Parameters.AddWithValue("@Sch_ID", schId);

                bool exists =
                    Convert.ToInt32(check.ExecuteScalar()) > 0;

                string query;

                if (exists)
                {
                    // UPDATE EXISTING SCHEDULE

                    query = @"
                        UPDATE ScheduleMaster
                        SET Sch_Date = @Sch_Date,
                            Order_ID = @Order_ID,
                            Sch_Index = 1,
                            Sch_Type = @Sch_Type,
                            Sch_Start_Time = @Sch_Start_Time
                        WHERE Sch_ID = @Sch_ID";
                }
                else
                {
                    // INSERT NEW SCHEDULE

                    query = @"
                        INSERT INTO ScheduleMaster
                        (
                            Sch_ID,
                            Sch_Date,
                            Order_ID,
                            Sch_Index,
                            Ordered_Qty,
                            Load_Send_Qty,
                            Status,
                            Sch_Type,
                            Sch_Start_Time
                        )
                        VALUES
                        (
                            @Sch_ID,
                            @Sch_Date,
                            @Order_ID,
                            1,
                            NULL,
                            NULL,
                            'N',
                            @Sch_Type,
                            @Sch_Start_Time
                        )";
                }

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Sch_ID", schId);

                cmd.Parameters.AddWithValue(
                    "@Sch_Date", dpScheduleDate.SelectedDate.Value);

                cmd.Parameters.AddWithValue("@Order_ID", orderIds);

                cmd.Parameters.AddWithValue(
                    "@Sch_Type", cmbScheduleType.Text);

                cmd.Parameters.AddWithValue(
                    "@Sch_Start_Time",
                    startTime.HasValue
                    ? startTime.Value
                    : DBNull.Value);

                cmd.ExecuteNonQuery();

                MessageBox.Show(
                    exists
                    ? "Schedule Updated Successfully."
                    : "Schedule Saved Successfully.");

                LoadScheduleIDs();

                cmbScheduleID.SelectedValue = schId;

                LoadSchedule(schId);

                LoadAvailableOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Schedule Save Failed: " + ex.Message);
            }
        }

        // =====================================
        // EXIT
        // =====================================

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
