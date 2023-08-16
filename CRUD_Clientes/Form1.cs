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
        private SqlConnection connection;
        public Form1()
        {
            InitializeComponent();

            TestConnection test = new TestConnection();
            connection = test.Connection(@"server = VINICIUSPIRESPC\SQLDEV2016 ;Database = Clientes ;User Id = sa ;Password = Sync1004inova;");

            ObterGenerosBanco obter = new ObterGenerosBanco();
            List<string> listaGeneros = obter.ObterGeneros(connection);
            comboBoxGenero.Items.Clear();
            comboBoxGenero.Items.AddRange(listaGeneros.ToArray());
            comboBoxGenero.SelectedIndex = 0;
            textBusca.Focus();
            textCodigo.Enabled = false;
            panelAlter.Visible = false;
            btnVoltar.Visible = false;
        }

        private void btnExibir_Click(object sender, EventArgs e)
        {
            if (dataGridClientes.DataSource is null)
            {
                ExibirCliente exibir = new ExibirCliente();
                DataSet ds = new DataSet();

                ds = exibir.ExibirClientes(connection, textBusca.Text, ds);

                dataGridClientes.DataSource = ds.Tables[0];

                foreach (DataGridViewColumn column in dataGridClientes.Columns)
                {
                    column.ReadOnly = true;
                }
                dataGridClientes.Focus();
            }
        }

        private async void AtribuiTexts(string codigo)
        {
            btnVoltar.Visible = true;

            if (string.IsNullOrEmpty(codigo))
            {
                btnAdicionar.PerformClick();
                return;
            }

            try
            {
                await connection.OpenAsync();

                SqlDataReader reader = null;
                ConsultaCliente consultaCliente = new ConsultaCliente();
                reader = await consultaCliente.Consultar(connection, codigo, reader);

                if (reader.Read())
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
                ShowErrorMessage($"Erro ao exibir o cliente: \n{sqlex}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erro ao exibir o cliente: \n{ex}");
            }
            finally
            {
                connection.Close();
            }

            label8.Text = "Alterar ou Excluir Cliente";
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                AtribuiTexts(textCodigo.Text);
                textBusca.Visible = false;
                panelAlter.Visible = true;
                btnVoltar.Visible = true;
                textNome.Focus();
            }
        }

        private async void btnSalvar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textCodigo.Text))
            {
                InserirCliente inserirCliente = new InserirCliente();
                await inserirCliente.Inserir(connection, textNome.Text, textSobrenome.Text, comboBoxGenero.ToString(), dateTimePicker1.Text, textEnd.Text, textNum.Text);
                ResetForm();
                return;
            }

            UpdateCliente updateCliente = new UpdateCliente();
            await updateCliente.Update(connection, textCodigo.Text, textNome.Text, textSobrenome.Text, comboBoxGenero.Text, dateTimePicker1.Text, textEnd.Text, textNum.Text);

            AtribuiTexts(textCodigo.Text);

            ResetForm();
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {

            DeleteCliente deleteCliente = new DeleteCliente();
            await deleteCliente.Delete(connection, textCodigo.Text, textNome.Text, textSobrenome.Text);

            ResetForm();
        }

        private void dataGridClientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int rowIndex = dataGridClientes.CurrentCell.RowIndex;
                DataGridViewRow selectedRow = dataGridClientes.Rows[rowIndex];
                string codigoCliente = selectedRow.Cells["Codigo do Cliente"].Value.ToString();

                textCodigo.Text = codigoCliente;

                dataGridClientes.Visible = false;

                AtribuiTexts(codigoCliente);
                textBusca.Visible = false;
                panelAlter.Visible = true;
                btnVoltar.Visible = true;
                textNome.Focus();

                // Impede que a tecla "Enter" continue sendo processada pela grade de dados
                e.Handled = true;
            }
        }
        private void ResetForm()
        {
            label8.Text = "Buscar Cliente";
            dataGridClientes.Visible = true;
            dataGridClientes.Focus();
            textBusca.Visible = true;
            panelAlter.Visible = false;
            btnVoltar.Visible = false;
            textNome.Text = "";
            textSobrenome.Text = "";
            comboBoxGenero.SelectedIndex = 0;
            dateTimePicker1.Text = DateTime.Now.ToString();
            textEnd.Text = "";
            textNum.Text = "";
            btnExibir.PerformClick();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            label8.ResetText();
            label8.Text = "Inserir novo cliente";
            btnVoltar.Visible = true;
            dataGridClientes.Visible = false;
            textBusca.Visible = false;
            panelAlter.Visible = true;
        }

        private void textBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Simula o clique do botão btnExibir
                btnExibir.PerformClick();
                dataGridClientes.Focus();
            }
        }

        private void comboBoxGenero_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void textNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSobrenome_TextChanged(object sender, EventArgs e)
        {

        }

        private void textNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void textEnd_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBusca_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
