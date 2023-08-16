using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_Clientes.Util
{
    public class TestConnection
    {
        public SqlConnection Connection(string ConnectionString)
        {

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            try
            {
                sqlConnection.Open();
                sqlConnection.Close();

                return sqlConnection;
            }
            catch (SqlException sqlex)
            {
                ShowErroMessage($"Erro ao conectar com o banco de dados. \n{sqlex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                ShowErroMessage($"Erro ao conectar com o banco de dados. \n{ex.Message}");
                return null;
            }
            finally
            {
                sqlConnection.Close(); 
            }
        }

        private void ShowErroMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
