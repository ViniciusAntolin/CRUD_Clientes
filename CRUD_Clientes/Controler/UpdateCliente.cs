using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class UpdateCliente
    {
        public async Task Update(SqlConnection connection, string codigo, string nome, string sobrenome, string comboBoxGenero, string datanascimento, string endereco, string num)
        {
            try
            {
                SqlDataReader reader = null;
                await connection.OpenAsync();
                ConsultaCliente consulta = new ConsultaCliente();
                using (reader = await consulta.Consultar(connection, codigo, reader))
                {
                    if (reader.Read())
                    {
                        // Recuperar os valores atuais do banco de dados
                        string nomeAtual = (string)reader[1];
                        string sobrenomeAtual = (string)reader[2];
                        string generoAtual = ((Byte)reader[3] == 1) ? "Masculino" :
                                             ((Byte)reader[3] == 2) ? "Feminino" : "Outro";
                        DateTime dataNascimentoAtual = (DateTime)(reader[4]);
                        string enderecoAtual = (string)reader[5];
                        int numeroAtual = Convert.ToInt32(reader[6]);

                        string dataFormatada = dataNascimentoAtual.ToString("dd/MM/yyyy");


                        // Comparar os valores recuperados com os valores passados como parâmetros
                        if (nome != nomeAtual || sobrenome != sobrenomeAtual || comboBoxGenero != generoAtual ||
                            datanascimento != dataFormatada || endereco != enderecoAtual ||
                            num != numeroAtual.ToString())
                        {
                            // Pelo menos um valor é diferente, então realizar a atualização
                            string updateSql = "UPDATE clientes SET Nome = @Nome, Sobrenome = @Sobrenome, CodigoGenero = @CodigoGenero, " +
                                               "DataNascimento = @DataNascimento, Endereco = @Endereco, Numero = @Numero WHERE CodigoCliente = @CodigoCliente";

                            using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@CodigoCliente", codigo);
                                updateCommand.Parameters.AddWithValue("@Nome", nome);
                                updateCommand.Parameters.AddWithValue("@Sobrenome", sobrenome);
                                updateCommand.Parameters.AddWithValue("@CodigoGenero", (comboBoxGenero == "Masculino") ? 1 :
                                                                                       (comboBoxGenero == "Feminino") ? 2 :
                                                                                       5);
                                updateCommand.Parameters.AddWithValue("@DataNascimento", DateTime.Parse(datanascimento));
                                updateCommand.Parameters.AddWithValue("@Endereco", endereco);
                                updateCommand.Parameters.AddWithValue("@Numero", Convert.ToInt32(num));
                                updateCommand.CommandTimeout = 30000;
                                await updateCommand.ExecuteNonQueryAsync();

                                ShowSucessMessage($"Cliente {nome + " " + sobrenome} alterado com sucesso!");
                            }
                        }
                        else
                        {
                            ShowWarningMessage($"Nenhuma alteração realizada para {nome + " " + sobrenome}, pois nenhuma informação foi alterada.");
                        }
                    }

                }

                connection.Close();
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
