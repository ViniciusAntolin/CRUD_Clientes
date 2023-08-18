using CRUD_Clientes.Model;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class InserirCliente
    {
        public async Task<long> Inserir(SqlConnection connection, ClienteModel model)
        {
            string InsertSql = "INSERT INTO [dbo].[Clientes] (Nome, Sobrenome, CodigoGenero, DataNascimento, Endereco, Numero) " +
                "OUTPUT INSERTED.CodigoCliente " +
                "VALUES (@Nome, @Sobrenome, @CodigoGenero, @DataNascimento, @Endereco, @Numero)";

            long novoCodigoCliente = -1;

            try
            {
                connection.Open();

                // Cria um comando SQL para realizar a inserção
                using (SqlCommand command = new SqlCommand(InsertSql, connection))
                {
                    // Definindo os parâmetros do comando SQL com os valores do modelo
                    command.Parameters.AddWithValue("@Nome", model.Nome);
                    command.Parameters.AddWithValue("@Sobrenome", model.Sobrenome);
                    command.Parameters.AddWithValue("@CodigoGenero", (model.Genero == "Masculino") ? 1 :
                                                                    (model.Genero == "Feminino") ? 2 :
                                                                    5);
                    command.Parameters.AddWithValue("@DataNascimento", model.DataNascimento);
                    command.Parameters.AddWithValue("@Endereco", model.Endereco);
                    command.Parameters.AddWithValue("@Numero", model.Numero);
                    command.CommandTimeout = 30000;

                    // Executa o comando SQL e obtém o novo código do cliente
                    novoCodigoCliente = (long)await command.ExecuteScalarAsync();
                }

                MessageBox.Show($"Cliente: {model.Nome + " " + model.Sobrenome}, cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show($"Erro ao cadastrar o cliente: {sqlex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar o cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            // Retorna o novo código do cliente inserido
            return novoCodigoCliente;
        }
    }
}
