using CRUD_Clientes.Model;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class ConsultaCliente
    {
        public async Task<ClienteModel> Consultar(SqlConnection connection, string codigo)
        {
            ClienteModel cliente = null;

            try
            {
                string BuscaClienteSql = "SELECT CodigoCliente, Nome, Sobrenome, Descricao, DataNascimento, Endereco, Numero FROM clientes C " +
                                         "INNER JOIN Generos G ON G.CodigoGenero = C.CodigoGenero WHERE CodigoCliente = @CodigoCliente";

                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(BuscaClienteSql, connection))
                {
                    command.Parameters.AddWithValue("@CodigoCliente", Convert.ToInt64(codigo));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            cliente = new ClienteModel
                            {
                                CodigoCliente = reader.GetInt64(0),
                                Nome = reader.GetString(1),
                                Sobrenome = reader.GetString(2),
                                Genero = reader.GetString(3),
                                DataNascimento = reader.GetDateTime(4),
                                Endereco = reader.GetString(5),
                                Numero = Convert.ToString(reader.GetInt32(6))
                            };
                        }
                    }
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
            finally 
            { 
                connection.Close();
            }

            return cliente;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
