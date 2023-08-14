using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class ObterGeneros
    {
        public List<string> ObterGenerosBanco(SqlConnection sqlConnection)
        {
            List<string> listaBancos = new List<string>();
            string buscaBanco = "SELECT Descricao FROM GENEROS";

            try
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand(buscaBanco, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                sqlConnection.Close();
            }

            return listaBancos;

        }
    }
}
