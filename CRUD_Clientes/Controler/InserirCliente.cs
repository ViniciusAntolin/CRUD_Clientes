using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class Inserir
    {

        public async Task InserirCliente(SqlConnection connection, string nome, string sobrenome, int codigogenero, DateTime datanascimento, string endereco, string numero)
        {
            string sql = $"INSERT INTO [dbo].[Clientes] (Nome, Sobrenome, CodigoGenero, DataNascimento, Endereco, Numero) " +
                "VALUES (@Nome, @Sobrenome, @CodigoGenero, @DataNascimento, @Endereco, @Numero)";
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nome", nome);
                    command.Parameters.AddWithValue("@Sobrenome", sobrenome);
                    command.Parameters.AddWithValue("@CodigoGenero", codigogenero);
                    command.Parameters.AddWithValue("@DataNascimento", datanascimento);
                    command.Parameters.AddWithValue("@Endereco", endereco);
                    command.Parameters.AddWithValue("@Numero", numero);
                    command.CommandTimeout = 1000;

                    await command.ExecuteNonQueryAsync();
                }
                MessageBox.Show($"Cliente: {nome + " " + sobrenome}, cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }
    }
}
