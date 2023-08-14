using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CRUD_Clientes.Controler
{
    public class ExibirCliente
    {
        public async Task<DataSet> ExibirClientes(SqlConnection connection)
        {
            try
            {
                string buscaClienteSql = "SELECT Nome + ' ' + Sobrenome as NomeCompleto, DataNascimento, g.Descricao FROM clientes c\r\nINNER JOIN genero g on c.genero = g.codigogenero";

                DataSet ds = new DataSet();

                SqlDataAdapter da = new SqlDataAdapter(buscaClienteSql, connection);

                await connection.OpenAsync();

                da.Fill(ds);

                return ds;

            }
            catch (SqlException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
