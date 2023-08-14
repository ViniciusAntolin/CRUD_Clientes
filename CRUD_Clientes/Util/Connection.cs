using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Util
{
    public class Connection
    {
        public SqlConnection TestConnection(string ConnectionString)
        {

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            try
            {
                sqlConnection.OpenAsync();
                sqlConnection.Close();

                return sqlConnection;
            }
            catch (SqlException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
