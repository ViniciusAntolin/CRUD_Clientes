using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class ExibirCliente
    {
        public DataSet ExibirClientes(SqlConnection connection, string Busca, DataSet ds)
        {
           SqlDataAdapter da = null;
            try
            {

                string buscaClienteSql = "select c.CodigoCliente as 'Codigo do Cliente', c.Nome + ' ' + c.Sobrenome as 'Nome Completo'" +
                    ", datediff(year, c.DataNascimento, getdate()) as Idade, g.Descricao from Clientes C " +
                    "inner join Generos g on g.CodigoGenero = c.CodigoGenero " +
                    "where (c.Nome + ' ' + c.Sobrenome like @Busca OR c.CodigoCliente like @Busca)";

                using (SqlCommand command = new SqlCommand(buscaClienteSql, connection))
                {
                    connection.Open();

                    if (long.TryParse(Busca, out long Codigo))
                    {
                        command.Parameters.AddWithValue("@Busca", Codigo);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Busca", "%" + Busca + "%");
                    }
                    command.CommandTimeout = 30000;

                    da = new SqlDataAdapter(command);
                }

                da.Fill(ds);

            }
            catch (SqlException sqlex)
            {
                ShowErrorMessage($"Erro ao buscar os clientes \n{sqlex}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erro ao buscar os clientes \n{ex}");
            }
            finally
            {
                connection.Close();
            }
            return ds;
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
