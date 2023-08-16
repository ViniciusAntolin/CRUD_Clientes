using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class ConsultaCliente
    {
        public Task<SqlDataReader> Consultar(SqlConnection connection, string codigo, SqlDataReader reader)
        {
            try
            {
                string BuscaClienteSql = "SELECT CodigoCliente, Nome, Sobrenome, CodigoGenero, DataNascimento, Endereco, Numero FROM clientes WHERE CodigoCliente = @CodigoCliente";

                using (SqlCommand command = new SqlCommand(BuscaClienteSql, connection))
                {
                    command.Parameters.AddWithValue("@CodigoCliente", Convert.ToInt64(codigo));
                    reader = command.ExecuteReader();
                }

            }
            catch (SqlException sqlex)
            {
                ShowErrorMessage($"Erro ao consultar o cliente: \n{sqlex}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erro ao consultar o cliente: \n{ex}");
            }

            return Task.FromResult(reader);
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
