using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_Clientes.Util
{
    public class TestConnection
    {
        public SqlConnection Connection(string ConnectionString)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
                connection.Close();

                return connection;
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
                connection.Close(); 
            }
        }

        private void ShowErroMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
