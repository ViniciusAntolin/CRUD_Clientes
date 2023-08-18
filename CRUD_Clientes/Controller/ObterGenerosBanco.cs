using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class ObterGenerosBanco
    {
        public List<string> ObterGeneros(SqlConnection connection)
        {
            List<string> listaBancos = new List<string>();
            string BuscaGeneroSql = "SELECT Descricao FROM GENEROS";

            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(BuscaGeneroSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        command.CommandTimeout = 30000;
                        while (reader.Read())
                        {
                            string databaseName = reader["Descricao"].ToString();
                            listaBancos.Add(databaseName);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao buscar os generos do banco: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            return listaBancos;

        }
    }
}
