﻿using CRUD_Clientes.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Clientes.Controler
{
    public class DeleteCliente
    {
        public async Task Delete(SqlConnection connection, string codigo)
        {
            try
            {
                string DeleteSql = "Delete from clientes WHERE CodigoCliente = @CodigoCliente";
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(DeleteSql, connection))
                {
                    command.Parameters.AddWithValue("@CodigoCliente", codigo);
                    command.CommandTimeout = 30000;

                    // Executa o comando de exclusão
                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                ShowErrorMessage($"Erro ao excluir o cliente: \n{sqlex.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erro ao excluir o cliente: \n{ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void ShowSucessMessage(string message)
        {
            MessageBox.Show(message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
