namespace Scada_Demo.Common
{
    public static class UserSession
    {
        // Basic Details
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static string FullName { get; set; }

        // ================= Master =================
        public static bool Auth_MAS_CUS { get; set; }
        public static bool Auth_MAS_SITE { get; set; }
        public static bool Auth_MAS_RECP { get; set; }
        public static bool Auth_MAS_TRK { get; set; }
        public static bool Auth_MAS_ORD { get; set; }
        public static bool Auth_MAS_SCH { get; set; }
        public static bool Auth_MAS_INW { get; set; }
        public static bool Auth_MAS_ALM { get; set; }
        public static bool Auth_MAS_MNT { get; set; }

        // ================= Others =================
        public static bool Auth_CRL { get; set; }
        public static bool Auth_IMP { get; set; }
        public static bool Auth_RST { get; set; }
        public static bool Auth_RCV { get; set; }

        // ================= Transactions =================
        public static bool Auth_TS_STB { get; set; }
        public static bool Auth_TS_PBD { get; set; }
        public static bool Auth_TS_PDK { get; set; }
        public static bool Auth_TS_AHV { get; set; }
        public static bool Auth_TS_DVC { get; set; }
        public static bool Auth_TS_RPV { get; set; }

        // ================= Transaction Settings =================
        public static bool Auth_RCA { get; set; }
        public static bool Auth_CAL { get; set; }
        public static bool Auth_GIF { get; set; }
        public static bool Auth_SMS { get; set; }
        public static bool Auth_DBH { get; set; }
        public static bool Auth_ANC { get; set; }
        public static bool Auth_WBC { get; set; }

        // ================= Batch Settings =================
        public static bool Auth_BS_TOL { get; set; }
        public static bool Auth_BS_DSQ { get; set; }
        public static bool Auth_BS_STP { get; set; }
        public static bool Auth_BS_EMPV { get; set; }
        public static bool Auth_BS_MIA { get; set; }
        public static bool Auth_BS_CTF { get; set; }
        public static bool Auth_BS_JT { get; set; }
        public static bool Auth_BS_BOM { get; set; }
        public static bool Auth_BS_GS{ get; set; }

        // ================= Service Parameters =================
        public static bool Auth_SP_MGP { get; set; }
        public static bool Auth_SP_MXP { get; set; }
        public static bool Auth_SP_SKP { get; set; }
        public static bool Auth_SP_CVP { get; set; }
        public static bool Auth_SP_MTP { get; set; }
        public static bool Auth_SP_VBP { get; set; }

        // ================= Other Settings =================
        public static bool Auth_OS_BIN { get; set; }
        public static bool Auth_OS_CAP { get; set; }
        public static bool Auth_OS_EDOM { get; set; }
        public static bool Auth_OS_IAIF { get; set; }
        public static bool Auth_OS_PPS { get; set; }

        public static void Clear()
        {
            UserId = 0;
            UserName = string.Empty;
            FullName = string.Empty;

            Auth_MAS_CUS = false;
            Auth_MAS_SITE = false;
            Auth_MAS_RECP = false;
            Auth_MAS_TRK = false;
            Auth_MAS_ORD = false;
            Auth_MAS_SCH = false;
            Auth_MAS_INW = false;
            Auth_MAS_ALM = false;
            Auth_MAS_MNT = false;

            Auth_CRL = false;
            Auth_IMP = false;
            Auth_RST = false;
            Auth_RCV = false;

            Auth_TS_STB = false;
            Auth_TS_PBD = false;
            Auth_TS_PDK = false;
            Auth_TS_AHV = false;
            Auth_TS_DVC = false;
            Auth_TS_RPV = false;

            Auth_RCA = false;
            Auth_CAL = false;
            Auth_GIF = false;
            Auth_SMS = false;
            Auth_DBH = false;
            Auth_ANC = false;
            Auth_WBC = false;

            Auth_BS_TOL = false;
            Auth_BS_DSQ = false;
            Auth_BS_STP = false;
            Auth_BS_EMPV = false;
            Auth_BS_MIA = false;
            Auth_BS_CTF = false;
            Auth_BS_JT = false;
            Auth_BS_BOM = false;
            Auth_BS_GS = false;

            Auth_SP_MGP = false;
            Auth_SP_MXP = false;
            Auth_SP_SKP = false;
            Auth_SP_CVP = false;
            Auth_SP_MTP = false;
            Auth_SP_VBP = false;

            Auth_OS_BIN = false;
            Auth_OS_CAP = false;
            Auth_OS_EDOM = false;
            Auth_OS_IAIF = false;
            Auth_OS_PPS = false;
        }
    }
}