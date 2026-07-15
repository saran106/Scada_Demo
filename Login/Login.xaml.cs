using System.Windows;
using Microsoft.Data.SqlClient;
using Scada_Demo.Common;

using Scada_Demo.Database;

namespace Scada_Demo.Login
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string query = @"
SELECT *
FROM U_Mas
WHERE UserName = @UserName
  AND dbo.ufn_Decryption(PasswordHash) = @Password
  AND IsActive = 1";

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserName", txtUser.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Password.Trim());

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // ================= Basic =================
                    UserSession.UserId = Convert.ToInt32(dr["User_ID"]);
                    UserSession.UserName = dr["UserName"].ToString();
                    UserSession.FullName = dr["FullName"].ToString();

                    // ================= Master =================
                    UserSession.Auth_MAS_CUS = Convert.ToBoolean(dr["Auth_MAS_CUS"]);
                    UserSession.Auth_MAS_SITE = Convert.ToBoolean(dr["Auth_MAS_SITE"]);
                    UserSession.Auth_MAS_RECP = Convert.ToBoolean(dr["Auth_MAS_RECP"]);
                    UserSession.Auth_MAS_TRK = Convert.ToBoolean(dr["Auth_MAS_TRK"]);
                    UserSession.Auth_MAS_ORD = Convert.ToBoolean(dr["Auth_MAS_ORD"]);
                    UserSession.Auth_MAS_SCH = Convert.ToBoolean(dr["Auth_MAS_SCH"]);
                    UserSession.Auth_MAS_INW = Convert.ToBoolean(dr["Auth_MAS_INW"]);
                    UserSession.Auth_MAS_ALM = Convert.ToBoolean(dr["Auth_MAS_ALM"]);
                    UserSession.Auth_MAS_MNT = Convert.ToBoolean(dr["Auth_MAS_MNT"]);

                    // ================= Others =================
                    UserSession.Auth_CRL = Convert.ToBoolean(dr["Auth_CRL"]);
                    UserSession.Auth_IMP = Convert.ToBoolean(dr["Auth_IMP"]);
                    UserSession.Auth_RST = Convert.ToBoolean(dr["Auth_RST"]);
                    UserSession.Auth_RCV = Convert.ToBoolean(dr["Auth_RCV"]);

                    // ================= Transactions =================
                    UserSession.Auth_TS_STB = Convert.ToBoolean(dr["Auth_TS_STB"]);
                    UserSession.Auth_TS_PBD = Convert.ToBoolean(dr["Auth_TS_PBD"]);
                    UserSession.Auth_TS_PDK = Convert.ToBoolean(dr["Auth_TS_PDK"]);
                    UserSession.Auth_TS_AHV = Convert.ToBoolean(dr["Auth_TS_AHV"]);
                    UserSession.Auth_TS_DVC = Convert.ToBoolean(dr["Auth_TS_DVC"]);
                    UserSession.Auth_TS_RPV = Convert.ToBoolean(dr["Auth_TS_RPV"]);

                    // ================= Transaction Settings =================
                    UserSession.Auth_RCA = Convert.ToBoolean(dr["Auth_RCA"]);
                    UserSession.Auth_CAL = Convert.ToBoolean(dr["Auth_CAL"]);
                    UserSession.Auth_GIF = Convert.ToBoolean(dr["Auth_GIF"]);
                    UserSession.Auth_SMS = Convert.ToBoolean(dr["Auth_SMS"]);
                    UserSession.Auth_DBH = Convert.ToBoolean(dr["Auth_DBH"]);
                    UserSession.Auth_ANC = Convert.ToBoolean(dr["Auth_ANC"]);
                    UserSession.Auth_WBC = Convert.ToBoolean(dr["Auth_WBC"]);

                    // ================= Batch Settings =================
                    UserSession.Auth_BS_TOL = Convert.ToBoolean(dr["Auth_BS_TOL"]);
                    UserSession.Auth_BS_DSQ = Convert.ToBoolean(dr["Auth_BS_DSQ"]);
                    UserSession.Auth_BS_STP = Convert.ToBoolean(dr["Auth_BS_STP"]);
                    UserSession.Auth_BS_EMPV = Convert.ToBoolean(dr["Auth_BS_EMPV"]);
                    UserSession.Auth_BS_MIA = Convert.ToBoolean(dr["Auth_BS_MIA"]);
                    UserSession.Auth_BS_CTF = Convert.ToBoolean(dr["Auth_BS_CTF"]);
                    UserSession.Auth_BS_JT = Convert.ToBoolean(dr["Auth_BS_JT"]);
                    UserSession.Auth_BS_BOM = Convert.ToBoolean(dr["Auth_BS_BOM"]);

                    // ================= Service Parameters =================
                    UserSession.Auth_SP_MGP = Convert.ToBoolean(dr["Auth_SP_MGP"]);
                    UserSession.Auth_SP_MXP = Convert.ToBoolean(dr["Auth_SP_MXP"]);
                    UserSession.Auth_SP_SKP = Convert.ToBoolean(dr["Auth_SP_SKP"]);
                    UserSession.Auth_SP_CVP = Convert.ToBoolean(dr["Auth_SP_CVP"]);
                    UserSession.Auth_SP_MTP = Convert.ToBoolean(dr["Auth_SP_MTP"]);
                    UserSession.Auth_SP_VBP = Convert.ToBoolean(dr["Auth_SP_VBP"]);

                    // ================= Other Settings =================
                    UserSession.Auth_OS_BIN = Convert.ToBoolean(dr["Auth_OS_BIN"]);
                    UserSession.Auth_OS_CAP = Convert.ToBoolean(dr["Auth_OS_CAP"]);
                    UserSession.Auth_OS_EDOM = Convert.ToBoolean(dr["Auth_OS_EDOM"]);
                    UserSession.Auth_OS_IAIF = Convert.ToBoolean(dr["Auth_OS_IAIF"]);
                    UserSession.Auth_OS_PPS = Convert.ToBoolean(dr["Auth_OS_PPS"]);

                    MainWindow main = new MainWindow();
                    main.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password",
                                    "Login",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}