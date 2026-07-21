using Microsoft.Data.SqlClient;
using Scada_Demo.Common;
using Scada_Demo.Database;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Scada_Demo.ViewModels.User
{
    public class ModifyuserViewModel : INotifyPropertyChanged
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

        public ICommand RegisterCommand { get; }
        public ICommand ExitCommand { get; }
        public ModifyuserViewModel()
        {
            RegisterCommand = new RelayCommand(_ => UpdateUser());
            ExitCommand = new RelayCommand(_ => Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is Scada_Demo.User.Modify_user)
                ?.Close());

            LoadUser();
        }

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

        private void LoadUser()
        {
            try
            {
                using SqlConnection con = DbConnection.GetConnection();
                con.Open();

                string query = @"
SELECT *,
       dbo.ufn_Decryption(PasswordHash) AS DecryptedPassword
FROM U_Mas
WHERE User_ID = @UserId";

                using SqlCommand cmd = new(query, con);
                cmd.Parameters.AddWithValue("@UserId", UserSession.UserId);

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    UserName = dr["UserName"].ToString() ?? "";
                    FullName = dr["FullName"].ToString() ?? "";
                    Password = dr["DecryptedPassword"].ToString() ?? "";
                    ConfirmPassword = Password;
                    AuthMasCus = Convert.ToBoolean(dr["Auth_MAS_CUS"]);
                    AuthMasSite = Convert.ToBoolean(dr["Auth_MAS_SITE"]);
                    AuthMasRecp = Convert.ToBoolean(dr["Auth_MAS_RECP"]);
                    AuthMasTrk = Convert.ToBoolean(dr["Auth_MAS_TRK"]);
                    AuthMasOrd = Convert.ToBoolean(dr["Auth_MAS_ORD"]);
                    AuthMasSch = Convert.ToBoolean(dr["Auth_MAS_SCH"]);
                    AuthMasInw = Convert.ToBoolean(dr["Auth_MAS_INW"]);
                    AuthMasAlm = Convert.ToBoolean(dr["Auth_MAS_ALM"]);
                    AuthMasMnt = Convert.ToBoolean(dr["Auth_MAS_MNT"]);

                    AuthBsTol = Convert.ToBoolean(dr["Auth_BS_TOL"]);
                    AuthBsDsq = Convert.ToBoolean(dr["Auth_BS_DSQ"]);
                    AuthBsStp = Convert.ToBoolean(dr["Auth_BS_STP"]);
                    AuthBsEmpv = Convert.ToBoolean(dr["Auth_BS_EMPV"]);
                    AuthBsMia = Convert.ToBoolean(dr["Auth_BS_MIA"]);
                    AuthBsCtf = Convert.ToBoolean(dr["Auth_BS_CTF"]);
                    AuthBsJt = Convert.ToBoolean(dr["Auth_BS_JT"]);
                    AuthBsBom = Convert.ToBoolean(dr["Auth_BS_BOM"]);
                    AuthBsGs = Convert.ToBoolean(dr["Auth_BS_GS"]); 

                    AuthSpMgp = Convert.ToBoolean(dr["Auth_SP_MGP"]);
                    AuthSpMxp = Convert.ToBoolean(dr["Auth_SP_MXP"]);
                    AuthSpSkp = Convert.ToBoolean(dr["Auth_SP_SKP"]);
                    AuthSpCvp = Convert.ToBoolean(dr["Auth_SP_CVP"]);
                    AuthSpMtp = Convert.ToBoolean(dr["Auth_SP_MTP"]);
                    AuthSpVbp = Convert.ToBoolean(dr["Auth_SP_VBP"]);


                    AuthOsBin = Convert.ToBoolean(dr["Auth_OS_BIN"]);
                    AuthOsCap = Convert.ToBoolean(dr["Auth_OS_CAP"]);
                    AuthOsEdom = Convert.ToBoolean(dr["Auth_OS_EDOM"]);
                    AuthOsIaif = Convert.ToBoolean(dr["Auth_OS_IAIF"]);
                    AuthOsPps = Convert.ToBoolean(dr["Auth_OS_PPS"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateUser()
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

                // Username unique check
                string checkQuery = @"SELECT COUNT(*)
                              FROM U_Mas
                              WHERE UserName = @UserName
                              AND User_ID <> @UserId";

                using SqlCommand checkCmd = new(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@UserName", UserName);
                checkCmd.Parameters.AddWithValue("@UserId", UserSession.UserId);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Username already exists.");
                    return;
                }

                string updateQuery = @"
UPDATE U_Mas
SET
UserName=@UserName,
PasswordHash=dbo.ufn_Encryption(@Password),
FullName=@FullName,

Auth_MAS_CUS=@AuthMasCus,
Auth_MAS_SITE=@AuthMasSite,
Auth_MAS_RECP=@AuthMasRecp,
Auth_MAS_TRK=@AuthMasTrk,
Auth_MAS_ORD=@AuthMasOrd,
Auth_MAS_SCH=@AuthMasSch,
Auth_MAS_INW=@AuthMasInw,
Auth_MAS_ALM=@AuthMasAlm,
Auth_MAS_MNT=@AuthMasMnt,

Auth_BS_TOL=@AuthBsTol,
Auth_BS_DSQ=@AuthBsDsq,
Auth_BS_STP=@AuthBsStp,
Auth_BS_EMPV=@AuthBsEmpv,
Auth_BS_MIA=@AuthBsMia,
Auth_BS_CTF=@AuthBsCtf,
Auth_BS_JT=@AuthBsJt,
Auth_BS_BOM=@AuthBsBom,
Auth_BS_GS=@AuthBsGs,

Auth_SP_MGP=@AuthSpMgp,
Auth_SP_MXP=@AuthSpMxp,
Auth_SP_SKP=@AuthSpSkp,
Auth_SP_CVP=@AuthSpCvp,
Auth_SP_MTP=@AuthSpMtp,
Auth_SP_VBP=@AuthSpVbp,

Auth_OS_BIN=@AuthOsBin,
Auth_OS_CAP=@AuthOsCap,
Auth_OS_EDOM=@AuthOsEdom,
Auth_OS_IAIF=@AuthOsIaif,
Auth_OS_PPS=@AuthOsPps

WHERE User_ID=@UserId";

                using SqlCommand cmd = new(updateQuery, con);

                cmd.Parameters.AddWithValue("@UserId", UserSession.UserId);
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

                MessageBox.Show("User updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}