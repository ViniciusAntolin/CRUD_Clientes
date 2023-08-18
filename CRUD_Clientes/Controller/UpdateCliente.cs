using CRUD_Clientes.Model;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class UpdateCliente
    {
        public async Task Update(SqlConnection connection, ClienteModel clienteNovo, string codigo)
        {
            try
            {
                ConsultaCliente consulta = new ConsultaCliente();
                ClienteModel clienteAtual = await consulta.Consultar(connection, codigo);

                await connection.OpenAsync();
                if (clienteAtual != null)
                {
                    // Comparar os valores recuperados com os valores passados como parâmetros
                    if (clienteNovo.Nome != clienteAtual.Nome || clienteNovo.Sobrenome != clienteAtual.Sobrenome || clienteNovo.Genero != clienteAtual.Genero ||
                        clienteNovo.DataNascimento != clienteAtual.DataNascimento || clienteNovo.Endereco != clienteAtual.Endereco ||
                        clienteNovo.Numero != clienteAtual.Numero)
                    {
                        // Pelo menos um valor é diferente, então realizar a atualização
                        string updateSql = "UPDATE clientes SET Nome = @Nome, Sobrenome = @Sobrenome, CodigoGenero = @CodigoGenero, " +
                                           "DataNascimento = @DataNascimento, Endereco = @Endereco, Numero = @Numero WHERE CodigoCliente = @CodigoCliente";

                        using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@CodigoCliente", codigo);
                            updateCommand.Parameters.AddWithValue("@Nome", clienteNovo.Nome);
                            updateCommand.Parameters.AddWithValue("@Sobrenome", clienteNovo.Sobrenome);
                            updateCommand.Parameters.AddWithValue("@CodigoGenero", (clienteNovo.Genero == "Masculino") ? 1 :
                                                                                   (clienteNovo.Genero == "Feminino") ? 2 :
                                                                                   5);
                            updateCommand.Parameters.AddWithValue("@DataNascimento", clienteNovo.DataNascimento);
                            updateCommand.Parameters.AddWithValue("@Endereco", clienteNovo.Endereco);
                            updateCommand.Parameters.AddWithValue("@Numero", clienteNovo.Numero);
                            updateCommand.CommandTimeout = 30000;
                            updateCommand.ExecuteNonQuery();

                            ShowSucessMessage($"Cliente {clienteNovo.Nome + " " + clienteNovo.Sobrenome} alterado com sucesso!");
                        }
                    }
                    else
                    {
                        ShowWarningMessage($"Nenhuma alteração realizada para {clienteNovo.Nome + " " + clienteNovo.Sobrenome}, pois nenhuma informação foi alterada.");
                        return;
                    }
                }
            }
            catch (SqlException sqlex)
            {
                ShowErrorMessage($"Erro ao alteração o cliente \n{sqlex.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erro ao alteração o cliente \n{ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ShowSucessMessage(string message)
        {
            MessageBox.Show(message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
