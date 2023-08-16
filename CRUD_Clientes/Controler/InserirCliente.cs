using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class InserirCliente
    {

        public async Task Inserir(SqlConnection connection, string nome, string sobrenome, string comboBoxGenero, string datanascimento, string endereco, string numero)
        {
            string InsertSql = "INSERT INTO [dbo].[Clientes] (Nome, Sobrenome, CodigoGenero, DataNascimento, Endereco, Numero) " +
                "VALUES (@Nome, @Sobrenome, @CodigoGenero, @DataNascimento, @Endereco, @Numero)";
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(InsertSql, connection))
                {
                    command.Parameters.AddWithValue("@Nome", nome);
                    command.Parameters.AddWithValue("@Sobrenome", sobrenome);
                    command.Parameters.AddWithValue("@CodigoGenero", (comboBoxGenero == "Masculino") ? 1 :
                                                                    (comboBoxGenero == "Feminino") ? 2 :
                                                                    5);
                    command.Parameters.AddWithValue("@DataNascimento", datanascimento);
                    command.Parameters.AddWithValue("@Endereco", endereco);
                    command.Parameters.AddWithValue("@Numero", numero);
                    command.CommandTimeout = 30000;
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
