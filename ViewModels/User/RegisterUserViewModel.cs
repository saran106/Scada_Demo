using Microsoft.Data.SqlClient;
using Scada_Demo.Common;
using Scada_Demo.Database;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Scada_Demo.ViewModels.User
{
    public class RegisterUserViewModel : INotifyPropertyChanged
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


        #region User Details

        private string _userName = string.Empty;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _fullName = string.Empty;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        #endregion

        #region Batch Settings Permissions

        private bool _authBsTol;
        public bool AuthBsTol
        {
            get => _authBsTol;
            set => SetProperty(ref _authBsTol, value);
        }

        private bool _authBsDsq;
        public bool AuthBsDsq
        {
            get => _authBsDsq;
            set => SetProperty(ref _authBsDsq, value);
        }

        private bool _authBsStp;
        public bool AuthBsStp
        {
            get => _authBsStp;
            set => SetProperty(ref _authBsStp, value);
        }

        private bool _authBsEmpv;
        public bool AuthBsEmpv
        {
            get => _authBsEmpv;
            set => SetProperty(ref _authBsEmpv, value);
        }

        private bool _authBsMia;
        public bool AuthBsMia
        {
            get => _authBsMia;
            set => SetProperty(ref _authBsMia, value);
        }

        private bool _authBsCtf;
        public bool AuthBsCtf
        {
            get => _authBsCtf;
            set => SetProperty(ref _authBsCtf, value);
        }

        private bool _authBsJt;
        public bool AuthBsJt
        {
            get => _authBsJt;
            set => SetProperty(ref _authBsJt, value);
        }

        private bool _authBsBom;
        public bool AuthBsBom
        {
            get => _authBsBom;
            set => SetProperty(ref _authBsBom, value);
        }

        private bool _authBsGs;
        public bool AuthBsGs
        {
            get => _authBsGs;
            set => SetProperty(ref _authBsGs, value);
        }

        #endregion

        #region Setup Parameters Permissions

        private bool _authSpMgp;
        public bool AuthSpMgp
        {
            get => _authSpMgp;
            set => SetProperty(ref _authSpMgp, value);
        }

        private bool _authSpMxp;
        public bool AuthSpMxp
        {
            get => _authSpMxp;
            set => SetProperty(ref _authSpMxp, value);
        }

        private bool _authSpSkp;
        public bool AuthSpSkp
        {
            get => _authSpSkp;
            set => SetProperty(ref _authSpSkp, value);
        }

        private bool _authSpCvp;
        public bool AuthSpCvp
        {
            get => _authSpCvp;
            set => SetProperty(ref _authSpCvp, value);
        }

        private bool _authSpMtp;
        public bool AuthSpMtp
        {
            get => _authSpMtp;
            set => SetProperty(ref _authSpMtp, value);
        }

        private bool _authSpVbp;
        public bool AuthSpVbp
        {
            get => _authSpVbp;
            set => SetProperty(ref _authSpVbp, value);
        }

        #endregion

        #region Other Setup Permissions

        private bool _authOsBin;
        public bool AuthOsBin
        {
            get => _authOsBin;
            set => SetProperty(ref _authOsBin, value);
        }

        private bool _authOsCap;
        public bool AuthOsCap
        {
            get => _authOsCap;
            set => SetProperty(ref _authOsCap, value);
        }

        private bool _authOsEdom;
        public bool AuthOsEdom
        {
            get => _authOsEdom;
            set => SetProperty(ref _authOsEdom, value);
        }

        private bool _authOsIaif;
        public bool AuthOsIaif
        {
            get => _authOsIaif;
            set => SetProperty(ref _authOsIaif, value);
        }

        private bool _authOsPps;
        public bool AuthOsPps
        {
            get => _authOsPps;
            set => SetProperty(ref _authOsPps, value);
        }

        #endregion

        #region Master Permissions

        private bool _authMasCus;
        public bool AuthMasCus
        {
            get => _authMasCus;
            set => SetProperty(ref _authMasCus, value);
        }

        private bool _authMasSite;
        public bool AuthMasSite
        {
            get => _authMasSite;
            set => SetProperty(ref _authMasSite, value);
        }

        private bool _authMasRecp;
        public bool AuthMasRecp
        {
            get => _authMasRecp;
            set => SetProperty(ref _authMasRecp, value);
        }

        private bool _authMasTrk;
        public bool AuthMasTrk
        {
            get => _authMasTrk;
            set => SetProperty(ref _authMasTrk, value);
        }

        private bool _authMasOrd;
        public bool AuthMasOrd
        {
            get => _authMasOrd;
            set => SetProperty(ref _authMasOrd, value);
        }

        private bool _authMasSch;
        public bool AuthMasSch
        {
            get => _authMasSch;
            set => SetProperty(ref _authMasSch, value);
        }

        private bool _authMasInw;
        public bool AuthMasInw
        {
            get => _authMasInw;
            set => SetProperty(ref _authMasInw, value);
        }

        private bool _authMasAlm;
        public bool AuthMasAlm
        {
            get => _authMasAlm;
            set => SetProperty(ref _authMasAlm, value);
        }

        private bool _authMasMnt;
        public bool AuthMasMnt
        {
            get => _authMasMnt;
            set => SetProperty(ref _authMasMnt, value);
        }

        #endregion

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }


        public ICommand RegisterCommand { get; }
        public ICommand ExitCommand { get; }


        private bool _selectAllPermissions;
        public bool SelectAllPermissions
        {
            get => _selectAllPermissions;
            set
            {
                if (SetProperty(ref _selectAllPermissions, value))
                {
                    SetAllPermissions(value);
                }
            }
        }
        public RegisterUserViewModel()
        {
            RegisterCommand = new RelayCommand(_ => RegisterUser());

            ExitCommand = new RelayCommand(_ =>
                Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is Scada_Demo.User.Register_New_User)
                ?.Close());
        }

        private void ClearForm()
        {
            UserName = "";
            FullName = "";
            Password = "";
            ConfirmPassword = "";

            AuthMasCus = false;
            AuthMasSite = false;
            AuthMasRecp = false;
            AuthMasTrk = false;
            AuthMasOrd = false;
            AuthMasSch = false;
            AuthMasInw = false;
            AuthMasAlm = false;
            AuthMasMnt = false;

            AuthBsTol = false;
            AuthBsDsq = false;
            AuthBsStp = false;
            AuthBsEmpv = false;
            AuthBsMia = false;
            AuthBsCtf = false;
            AuthBsJt = false;
            AuthBsBom = false;
            AuthBsGs = false;

            AuthSpMgp = false;
            AuthSpMxp = false;
            AuthSpSkp = false;
            AuthSpCvp = false;
            AuthSpMtp = false;
            AuthSpVbp = false;

            AuthOsBin = false;
            AuthOsCap = false;
            AuthOsEdom = false;
            AuthOsIaif = false;
            AuthOsPps = false;
        }

        private void SetAllPermissions(bool value)
        {

            SelectAllPermissions = false;
            // Master
            AuthMasCus = value;
            AuthMasSite = value;
            AuthMasRecp = value;
            AuthMasTrk = value;
            AuthMasOrd = value;
            AuthMasSch = value;
            AuthMasInw = value;
            AuthMasAlm = value;
            AuthMasMnt = value;

            // Batch Settings
            AuthBsTol = value;
            AuthBsDsq = value;
            AuthBsStp = value;
            AuthBsEmpv = value;
            AuthBsMia = value;
            AuthBsCtf = value;
            AuthBsJt = value;
            AuthBsBom = value;
            AuthBsGs = value;

            // Setup Parameters
            AuthSpMgp = value;
            AuthSpMxp = value;
            AuthSpSkp = value;
            AuthSpCvp = value;
            AuthSpMtp = value;
            AuthSpVbp = value;

            // Other Setup
            AuthOsBin = value;
            AuthOsCap = value;
            AuthOsEdom = value;
            AuthOsIaif = value;
            AuthOsPps = value;
        }

        private void RegisterUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    MessageBox.Show("Enter User Name.");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Password and Confirm Password do not match.");
                    return;
                }

                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                // Username already exists?
                string checkQuery = @"SELECT COUNT(*)
                              FROM U_Mas
                              WHERE UserName=@UserName";

                using SqlCommand checkCmd = new(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@UserName", UserName);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Username already exists.");
                    return;
                }

                string insertQuery = @"
INSERT INTO U_Mas
(
UserName,
PasswordHash,
FullName,

Auth_MAS_CUS,
Auth_MAS_SITE,
Auth_MAS_RECP,
Auth_MAS_TRK,
Auth_MAS_ORD,
Auth_MAS_SCH,
Auth_MAS_INW,
Auth_MAS_ALM,
Auth_MAS_MNT,

Auth_BS_TOL,
Auth_BS_DSQ,
Auth_BS_STP,
Auth_BS_EMPV,
Auth_BS_MIA,
Auth_BS_CTF,
Auth_BS_JT,
Auth_BS_BOM,
Auth_BS_GS,

Auth_SP_MGP,
Auth_SP_MXP,
Auth_SP_SKP,
Auth_SP_CVP,
Auth_SP_MTP,
Auth_SP_VBP,

Auth_OS_BIN,
Auth_OS_CAP,
Auth_OS_EDOM,
Auth_OS_IAIF,
Auth_OS_PPS
)
VALUES
(
@UserName,
dbo.ufn_Encryption(@Password),
@FullName,

@AuthMasCus,
@AuthMasSite,
@AuthMasRecp,
@AuthMasTrk,
@AuthMasOrd,
@AuthMasSch,
@AuthMasInw,
@AuthMasAlm,
@AuthMasMnt,

@AuthBsTol,
@AuthBsDsq,
@AuthBsStp,
@AuthBsEmpv,
@AuthBsMia,
@AuthBsCtf,
@AuthBsJt,
@AuthBsBom,
@AuthBsGs,

@AuthSpMgp,
@AuthSpMxp,
@AuthSpSkp,
@AuthSpCvp,
@AuthSpMtp,
@AuthSpVbp,

@AuthOsBin,
@AuthOsCap,
@AuthOsEdom,
@AuthOsIaif,
@AuthOsPps
)";

                using SqlCommand cmd = new(insertQuery, con);

                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@FullName", FullName);

                cmd.Parameters.AddWithValue("@AuthMasCus", AuthMasCus);
                cmd.Parameters.AddWithValue("@AuthMasSite", AuthMasSite);
                cmd.Parameters.AddWithValue("@AuthMasRecp", AuthMasRecp);
                cmd.Parameters.AddWithValue("@AuthMasTrk", AuthMasTrk);
                cmd.Parameters.AddWithValue("@AuthMasOrd", AuthMasOrd);
                cmd.Parameters.AddWithValue("@AuthMasSch", AuthMasSch);
                cmd.Parameters.AddWithValue("@AuthMasInw", AuthMasInw);
                cmd.Parameters.AddWithValue("@AuthMasAlm", AuthMasAlm);
                cmd.Parameters.AddWithValue("@AuthMasMnt", AuthMasMnt);

                cmd.Parameters.AddWithValue("@AuthBsTol", AuthBsTol);
                cmd.Parameters.AddWithValue("@AuthBsDsq", AuthBsDsq);
                cmd.Parameters.AddWithValue("@AuthBsStp", AuthBsStp);
                cmd.Parameters.AddWithValue("@AuthBsEmpv", AuthBsEmpv);
                cmd.Parameters.AddWithValue("@AuthBsMia", AuthBsMia);
                cmd.Parameters.AddWithValue("@AuthBsCtf", AuthBsCtf);
                cmd.Parameters.AddWithValue("@AuthBsJt", AuthBsJt);
                cmd.Parameters.AddWithValue("@AuthBsBom", AuthBsBom);
                cmd.Parameters.AddWithValue("@AuthBsGs", AuthBsGs);

                cmd.Parameters.AddWithValue("@AuthSpMgp", AuthSpMgp);
                cmd.Parameters.AddWithValue("@AuthSpMxp", AuthSpMxp);
                cmd.Parameters.AddWithValue("@AuthSpSkp", AuthSpSkp);
                cmd.Parameters.AddWithValue("@AuthSpCvp", AuthSpCvp);
                cmd.Parameters.AddWithValue("@AuthSpMtp", AuthSpMtp);
                cmd.Parameters.AddWithValue("@AuthSpVbp", AuthSpVbp);

                cmd.Parameters.AddWithValue("@AuthOsBin", AuthOsBin);
                cmd.Parameters.AddWithValue("@AuthOsCap", AuthOsCap);
                cmd.Parameters.AddWithValue("@AuthOsEdom", AuthOsEdom);
                cmd.Parameters.AddWithValue("@AuthOsIaif", AuthOsIaif);
                cmd.Parameters.AddWithValue("@AuthOsPps", AuthOsPps);

                cmd.ExecuteNonQuery();

                MessageBox.Show("User Registered Successfully.");

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}