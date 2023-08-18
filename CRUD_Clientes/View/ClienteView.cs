using CRUD_Clientes.Controler;
using CRUD_Clientes.Controller;
using CRUD_Clientes.Model;
using CRUD_Clientes.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_Clientes
{
    public partial class ClienteView : Form
    {
        private ClienteController _controller;
        public SqlConnection connection;
        public event EventHandler GridDoubleClick;
        public ClienteView()
        {
            InitializeComponent();

            _controller = new ClienteController(new ClienteModel(), this, new ConsultaCliente(), new UpdateCliente(), new InserirCliente(), new DeleteCliente(), new ExibirCliente()); ;
            _controller.FormsAdicionar += btnAdicionar_Click;
        }

        private void btnExibir_Click(object sender, EventArgs e)
        {
            _controller.btnExibir_Click(sender, e);
        }

        public void MostrarDetalhesCliente(ClienteModel cliente)
        {
            // Preencha campos do formulário com os dados do cliente
            textCodigo.Text = cliente.CodigoCliente.ToString("");
            textNome.Text = cliente.Nome;
            textSobrenome.Text = cliente.Sobrenome;
            comboBoxGenero.SelectedIndex = (cliente.Genero == "Masculino") ? 0 :
                                           (cliente.Genero == "Feminino") ? 1 :
                                           2;
            dateTimePicker1.Text = cliente.DataNascimento.ToString("dd/MM/yyyy");
            textEnd.Text = cliente.Endereco;
            textNum.Text = cliente.Numero;
            label8.Text = "Alterar ou Excluir Cliente";
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
                GridDoubleClick?.Invoke(this, e);
                textBusca.Visible = false;
                panelAlter.Visible = true;
                btnVoltar.Visible = true;
                textNome.Focus();
            }
        }

        public string GetCodigo()
        {
            return textCodigo.Text;
        }

        public string GetBusca()
        {
            return textBusca.Text;
        }
        public ClienteModel ConstruirModel()
        {
            var model = new ClienteModel();
            //model.CodigoCliente = Convert.ToInt64(textCodigo.Text);
            model.Nome = textNome.Text;
            model.Sobrenome = textSobrenome.Text;
            model.Genero = comboBoxGenero.Text;
            model.DataNascimento = dateTimePicker1.Value;
            model.Endereco = textEnd.Text;
            model.Numero = textNum.Text;
            return model;

        }
        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textNome.Text))
            {
                if (string.IsNullOrEmpty(GetCodigo()))
                {

                    _controller.btnInserir_Click(sender, e);
                    return;
                }

                _controller.btnSalvar_Click(sender, e);

                ResetForm();
                return;
            }
            MessageBox.Show("Não é possível cadastrar um cliente sem informar o Nome, por favor, validar se as informações realmente foram digitas", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textCodigo.Text))
            {
                _controller.btnExcluir_Click(sender, e);
            }

            MessageBox.Show("Não é possível excluir um cliente sem informar consultar, por favor, clique duas vezes no cliente que deseja Alterar/Excluir", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ResetForm();
        }

        private void dataGridClientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int rowIndex = dataGridClientes.CurrentCell.RowIndex;
                DataGridViewRow selectedRow = dataGridClientes.Rows[rowIndex];
                textCodigo.Text = selectedRow.Cells["Codigo do Cliente"].Value.ToString();

                dataGridClientes.Visible = false;

                GridDoubleClick?.Invoke(this, EventArgs.Empty);
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
            textCodigo.Text = "";
            textNome.Text = "";
            textSobrenome.Text = "";
            comboBoxGenero.SelectedIndex = 0;
            dateTimePicker1.Text = DateTime.Now.ToString();
            textEnd.Text = "";
            textNum.Text = "";
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

    }
}
