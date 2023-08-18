using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class ObterGenerosBanco
    {
        // Método para obter os gêneros dos clientes a partir do banco de dados
        public List<string> ObterGeneros(SqlConnection connection)
        {
            List<string> listaBancos = new List<string>();
            string BuscaGeneroSql = "SELECT Descricao FROM GENEROS";

            try
            {
                connection.Open();

                // Cria um comando SQL para executar a consulta
                using (SqlCommand command = new SqlCommand(BuscaGeneroSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        command.CommandTimeout = 30000;

                        // Lê cada registro retornado pela consulta
                        while (reader.Read())
                        {
                            // Obtém o valor da coluna "Descricao" e adiciona à lista
                            string databaseName = reader["Descricao"].ToString();
                            listaBancos.Add(databaseName);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao buscar os gêneros do banco: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            // Retorna a lista de gêneros obtida do banco de dados
            return listaBancos;
        }
    }
}
