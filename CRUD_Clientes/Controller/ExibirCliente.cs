using System;
using System.Data;
using System.Data.SqlClient;
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

                    // Verifica se a busca pode ser interpretada como um número de cliente
                    if (long.TryParse(Busca, out long Codigo))
                    {
                        command.Parameters.AddWithValue("@Busca", Codigo);
                    }
                    else
                    {
                        // Se não for um número, a busca será considerada como parte do nome
                        command.Parameters.AddWithValue("@Busca", "%" + Busca + "%");
                    }
                    command.CommandTimeout = 30000;

                    // Cria um SqlDataAdapter para preencher o DataSet
                    da = new SqlDataAdapter(command);
                }

                // Preenche o DataSet com os resultados da consulta
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
            // Retorna o DataSet contendo os resultados da busca
            return ds;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
