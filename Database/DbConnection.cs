using Microsoft.Data.SqlClient;

namespace Scada_Demo.Database
{
    public static class DbConnection
    {
        public static readonly string ConnectionString =
            @"Server=DESKTOP-C2DO8MF\SQLEXPRESS02;
              Database=Scada_Elantris;
              Trusted_Connection=True;
              TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}