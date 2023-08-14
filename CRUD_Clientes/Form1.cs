using CRUD_Clientes.Controler;
using CRUD_Clientes.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_Clientes
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        string genero;
        public Form1()
        {
            InitializeComponent();

            Connection connections = new Connection();
            connection = connections.TestConnection(@"server = VINICIUSPIRESPC\SQLDEV2016 ;Database = Clientes ;User Id = sa ;Password = Sync1004inova;");

            ObterGeneros obter = new ObterGeneros();
            List<string> listaGeneros = obter.ObterGenerosBanco(connection);
            comboBoxGenero.Items.Clear();
            comboBoxGenero.Items.AddRange(listaGeneros.ToArray());
            comboBoxGenero.SelectedIndex = 0;

            textCodigo.Enabled = false;
            panelAlter.Visible = false;
            btnVoltar.Visible = false;
        }
        private async void btnExibir_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = null;
            try
            {
                string buscaClienteSql = "select CodigoCliente as 'Codigo do Cliente', Nome + ' ' + Sobrenome as 'Nome Completo', datediff(year, DataNascimento, getdate()) as Idade, Descricao from Clientes C " +
                    "inner join Generos g on g.CodigoGenero = c.CodigoGenero where c.Nome + ' ' + c.Sobrenome like @Nome";

                using (SqlCommand command = new SqlCommand(buscaClienteSql, connection))
                {
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@Nome", "%" + textBuscaNome.Text + "%");
                    command.CommandTimeout = 30000;

                    da = new SqlDataAdapter(command);
                }

                da.Fill(ds);
                dataGridClientes.DataSource = ds.Tables[0];

                foreach (DataGridViewColumn column in dataGridClientes.Columns)
                {
                    column.ReadOnly = true;
                }

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

        }

        private void dataGridClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private async void Consultar(string codigo)
        {
            btnVoltar.Visible = true;


            if (string.IsNullOrEmpty(codigo))
            {
                EventArgs eventArgs = new EventArgs();
                btnAdicionar_Click(this, eventArgs);
                return;
            }

            label8.Text = "Alterar ou Excluir Cliente";

            try
            {
                SqlDataReader reader = null;

                string buscaClienteSql = "SELECT CodigoCliente, Nome, Sobrenome, CodigoGenero, DataNascimento, Endereco, Numero FROM clientes WHERE CodigoCliente = @CodigoCliente";

                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(buscaClienteSql, connection))
                {
                    command.Parameters.AddWithValue("@CodigoCliente", Convert.ToInt64(codigo));
                    reader = command.ExecuteReader();
                }

                while (reader.Read())
                {
                    textCodigo.Text = Convert.ToString(reader[0]);
                    textNome.Text = (string)reader[1];
                    textSobrenome.Text = (string)reader[2];
                    comboBoxGenero.SelectedIndex = ((Byte)reader[3] == 1) ? 0 : 
                                                    ((Byte)reader[3] == 2) ? 1 
                                                    : 2;
                    dateTimePicker1.Text = Convert.ToString(reader[4]);
                    textEnd.Text = (string)reader[5];
                    textNum.Text = Convert.ToString(reader[6]);
                }

            }
            catch (SqlException sqlex)
            {
                ShowErrorMessage($"Erro ao consultar o cliente \n{sqlex}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erro ao consultar o cliente \n{ex}");
            }
            finally
            {
                connection.Close();
            }
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSucessMessage(string message)
        {
            MessageBox.Show(message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dataGridClientes_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifique se uma linha foi selecionada
            if (e.RowIndex >= 0)
            {
                // Obtenha os dados da linha selecionada
                DataGridViewRow selectedRow = dataGridClientes.Rows[e.RowIndex];
                textCodigo.Text = selectedRow.Cells["Codigo do Cliente"].Value.ToString();

                dataGridClientes.Visible = false;

                Consultar(textCodigo.Text);
                textBuscaNome.Visible = false;
                panelAlter.Visible = true;
                btnVoltar.Visible = true;
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textCodigo.Text))
            {
                int codigogen;

                if (genero == "Masculino")
                {
                    codigogen = 1;
                }
                else
                {
                    codigogen = 2;
                }

                DateTime datanascimento = DateTime.Parse(dateTimePicker1.Text);

                //Connection connections = new Connection();
                //connection = connections.TestConnection(@"server = VINICIUSPIRESPC\SQLDEV2016 ;Database = PROJETO ;User Id = sa ;Password = Sync1004inova;");

                Inserir inserir = new Inserir();
                await inserir.InserirCliente(connection, textNome.Text, textSobrenome.Text, codigogen, datanascimento, textEnd.Text, textNum.Text);
                ResetForm();
                return;
                
            }

            try
            {
                string buscaClienteSql = "UPDATE clientes set Nome = @Nome, Sobrenome = @Sobrenome, CodigoGenero = @CodigoGenero, DataNascimento = @DataNascimento," +
                    " Endereco = @Endereco, Numero = @Numero WHERE CodigoCliente = @CodigoCliente";

                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(buscaClienteSql, connection))
                {
                    command.Parameters.AddWithValue("@CodigoCliente", textCodigo.Text);
                    command.Parameters.AddWithValue("@Nome", textNome.Text);
                    command.Parameters.AddWithValue("@Sobrenome", textSobrenome.Text);
                    command.Parameters.AddWithValue("@CodigoGenero",(comboBoxGenero.SelectedItem.ToString() == "Masculino") ? 1 :
                                                                    (comboBoxGenero.SelectedItem.ToString() == "Feminino") ? 2 :
                                                                    3);
                    command.Parameters.AddWithValue("@DataNascimento", DateTime.Parse(dateTimePicker1.Text));
                    command.Parameters.AddWithValue("@Endereco", textEnd.Text);
                    command.Parameters.AddWithValue("@Numero", Convert.ToInt32(textNum.Text));
                    await command.ExecuteNonQueryAsync();
                }

                ShowSucessMessage($"Cliente {textNome.Text} atualizado com sucesso!");

                connection.Close();

                Consultar(textCodigo.Text);

            }
            catch (SqlException sqlex)
            {

                ShowErrorMessage($"Erro ao atualizar o cliente \n{sqlex.Message}");
            }
            catch (Exception ex)
            {

                ShowErrorMessage($"Erro ao atualizar o cliente \n{ex.Message}");
            }
            finally
            {
                ResetForm();
                connection.Close();
            }
           
        }
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textCodigo.Text))
            {
                ShowWarningMessage($"Você não consultou nenhum cliente para realizar a altreção dele.");
                return;
            }

            try
            {
                string buscaClienteSql = "Delete from clientes WHERE CodigoCliente = @CodigoCliente";

                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(buscaClienteSql, connection))
                {
                    command.Parameters.AddWithValue("@CodigoCliente", textCodigo.Text);
                    await command.ExecuteNonQueryAsync();
                }

                ShowSucessMessage($"Cliente {textNome.Text + " " + textSobrenome.Text}, excluido do Banco de dados com sucesso");
                
            }
            catch (SqlException sqlex)
            {

                ShowErrorMessage($"Erro ao excluir o cliente \n{sqlex.Message}");
            }
            catch (Exception ex)
            {

                ShowErrorMessage($"Erro ao excluir o cliente \n{ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            ResetForm();
        }

        private void ResetForm()
        {
            label8.Text = "Buscar Por Nome";
            dataGridClientes.Visible = true;
            textBuscaNome.Visible = true;
            panelAlter.Visible = false;
            btnVoltar.Visible = false;
            textNome.Text = "";
            textSobrenome.Text = "";
            comboBoxGenero.SelectedIndex = 0;
            dateTimePicker1.Text = DateTime.Now.ToString();
            textEnd.Text = "";
            textNum.Text = "";

            Button button = new Button();
            EventArgs args  = new EventArgs();
            btnExibir_Click(button, args);
        }

        private void Btnvoltar_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            label8.ResetText();
            label8.Text = "Inserir novo cliente";
            btnVoltar.Visible = true;
            dataGridClientes.Visible = false;
            textBuscaNome.Visible = false;
            panelAlter.Visible = true;
        }

        private void comboBoxGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGenero.SelectedIndex >= 0)
            {
                genero = comboBoxGenero.SelectedItem.ToString();
            }
        }

        private void textBuscaNome_Enter(object sender, EventArgs e)
        {
            btnExibir_Click(sender, e);
        }
    }
}
