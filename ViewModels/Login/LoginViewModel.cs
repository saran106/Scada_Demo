using Microsoft.Data.SqlClient;
using Scada_Demo.Common;
using Scada_Demo.Database;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Scada_Demo.ViewModels.Login
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => Login());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        #endregion

        #region Properties

        private string _userName = string.Empty;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }

        public ICommand CancelCommand { get; }

        #endregion

        #region Login

        private void Login()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string query = @"
SELECT *
FROM U_Mas
WHERE UserName = @UserName
AND dbo.ufn_Decryption(PasswordHash)=@Password
AND IsActive=1";

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserName", UserName.Trim());
                cmd.Parameters.AddWithValue("@Password", Password.Trim());

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    //================ BASIC =================//

                    UserSession.UserId = Convert.ToInt32(dr["User_ID"]);
                    UserSession.UserName = dr["UserName"].ToString();
                    UserSession.FullName = dr["FullName"].ToString();

                    //================ MASTER =================//

                    UserSession.Auth_MAS_CUS = Convert.ToBoolean(dr["Auth_MAS_CUS"]);
                    UserSession.Auth_MAS_SITE = Convert.ToBoolean(dr["Auth_MAS_SITE"]);
                    UserSession.Auth_MAS_RECP = Convert.ToBoolean(dr["Auth_MAS_RECP"]);
                    UserSession.Auth_MAS_TRK = Convert.ToBoolean(dr["Auth_MAS_TRK"]);
                    UserSession.Auth_MAS_ORD = Convert.ToBoolean(dr["Auth_MAS_ORD"]);
                    UserSession.Auth_MAS_SCH = Convert.ToBoolean(dr["Auth_MAS_SCH"]);
                    UserSession.Auth_MAS_INW = Convert.ToBoolean(dr["Auth_MAS_INW"]);
                    UserSession.Auth_MAS_ALM = Convert.ToBoolean(dr["Auth_MAS_ALM"]);
                    UserSession.Auth_MAS_MNT = Convert.ToBoolean(dr["Auth_MAS_MNT"]);

                    //================ OTHERS =================//

                    UserSession.Auth_CRL = Convert.ToBoolean(dr["Auth_CRL"]);
                    UserSession.Auth_IMP = Convert.ToBoolean(dr["Auth_IMP"]);
                    UserSession.Auth_RST = Convert.ToBoolean(dr["Auth_RST"]);
                    UserSession.Auth_RCV = Convert.ToBoolean(dr["Auth_RCV"]);

                    //================ TRANSACTIONS =================//

                    UserSession.Auth_TS_STB = Convert.ToBoolean(dr["Auth_TS_STB"]);
                    UserSession.Auth_TS_PBD = Convert.ToBoolean(dr["Auth_TS_PBD"]);
                    UserSession.Auth_TS_PDK = Convert.ToBoolean(dr["Auth_TS_PDK"]);
                    UserSession.Auth_TS_AHV = Convert.ToBoolean(dr["Auth_TS_AHV"]);
                    UserSession.Auth_TS_DVC = Convert.ToBoolean(dr["Auth_TS_DVC"]);
                    UserSession.Auth_TS_RPV = Convert.ToBoolean(dr["Auth_TS_RPV"]);

                    //================ TRANSACTION SETTINGS =================//

                    UserSession.Auth_RCA = Convert.ToBoolean(dr["Auth_RCA"]);
                    UserSession.Auth_CAL = Convert.ToBoolean(dr["Auth_CAL"]);
                    UserSession.Auth_GIF = Convert.ToBoolean(dr["Auth_GIF"]);
                    UserSession.Auth_SMS = Convert.ToBoolean(dr["Auth_SMS"]);
                    UserSession.Auth_DBH = Convert.ToBoolean(dr["Auth_DBH"]);
                    UserSession.Auth_ANC = Convert.ToBoolean(dr["Auth_ANC"]);
                    UserSession.Auth_WBC = Convert.ToBoolean(dr["Auth_WBC"]);

                    //================ BATCH SETTINGS =================//

                    UserSession.Auth_BS_TOL = Convert.ToBoolean(dr["Auth_BS_TOL"]);
                    UserSession.Auth_BS_DSQ = Convert.ToBoolean(dr["Auth_BS_DSQ"]);
                    UserSession.Auth_BS_STP = Convert.ToBoolean(dr["Auth_BS_STP"]);
                    UserSession.Auth_BS_EMPV = Convert.ToBoolean(dr["Auth_BS_EMPV"]);
                    UserSession.Auth_BS_MIA = Convert.ToBoolean(dr["Auth_BS_MIA"]);
                    UserSession.Auth_BS_CTF = Convert.ToBoolean(dr["Auth_BS_CTF"]);
                    UserSession.Auth_BS_JT = Convert.ToBoolean(dr["Auth_BS_JT"]);
                    UserSession.Auth_BS_BOM = Convert.ToBoolean(dr["Auth_BS_BOM"]);
                    UserSession.Auth_BS_GS = Convert.ToBoolean(dr["Auth_BS_GS"]);

                    //================ SERVICE PARAMETERS =================//

                    UserSession.Auth_SP_MGP = Convert.ToBoolean(dr["Auth_SP_MGP"]);
                    UserSession.Auth_SP_MXP = Convert.ToBoolean(dr["Auth_SP_MXP"]);
                    UserSession.Auth_SP_SKP = Convert.ToBoolean(dr["Auth_SP_SKP"]);
                    UserSession.Auth_SP_CVP = Convert.ToBoolean(dr["Auth_SP_CVP"]);
                    UserSession.Auth_SP_MTP = Convert.ToBoolean(dr["Auth_SP_MTP"]);
                    UserSession.Auth_SP_VBP = Convert.ToBoolean(dr["Auth_SP_VBP"]);

                    //================ OTHER SETTINGS =================//

                    UserSession.Auth_OS_BIN = Convert.ToBoolean(dr["Auth_OS_BIN"]);
                    UserSession.Auth_OS_CAP = Convert.ToBoolean(dr["Auth_OS_CAP"]);
                    UserSession.Auth_OS_EDOM = Convert.ToBoolean(dr["Auth_OS_EDOM"]);
                    UserSession.Auth_OS_IAIF = Convert.ToBoolean(dr["Auth_OS_IAIF"]);
                    UserSession.Auth_OS_PPS = Convert.ToBoolean(dr["Auth_OS_PPS"]);

                    //================ OPEN MAIN WINDOW =================//

                    MainWindow main = new MainWindow();
                    main.Show();

                    // Close Login Window
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is Scada_Demo.Login.Login)
                        {
                            window.Close();
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password",
                                    "Login",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    Password = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cancel()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}