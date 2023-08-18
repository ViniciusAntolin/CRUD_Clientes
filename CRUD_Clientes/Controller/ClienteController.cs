using CRUD_Clientes.Controler;
using CRUD_Clientes.Model;
using System;
using System.Data;
using System.Windows.Forms;

namespace CRUD_Clientes.Controller
{
    public class ClienteController
    {
        private ClienteModel _cliente;
        private ClienteView _clienteView;
        private ConsultaCliente _consultaCliente;
        private UpdateCliente _updateCliente;
        private InserirCliente _inserirCliente;
        private DeleteCliente _deleteCliente;
        private ExibirCliente _exibirCliente;
        public EventHandler FormsAdicionar;
        public ClienteController(ClienteModel cliente, ClienteView clienteView, ConsultaCliente consultaCliente, UpdateCliente updateCliente, InserirCliente inserirCliente, DeleteCliente deleteCliente, ExibirCliente exibirCliente)
        {
            _cliente = cliente;
            _clienteView = clienteView;
            _consultaCliente = consultaCliente;
            _updateCliente = updateCliente;
            _inserirCliente = inserirCliente;
            _deleteCliente = deleteCliente;
            _exibirCliente = exibirCliente;

            _clienteView.GridDoubleClick += btnConsultar_Click;
        }

        public async void btnConsultar_Click(object sender, EventArgs e)
        {
            if (_clienteView.GetCodigo() != "")
            {
                // Consulta o cliente e obtém os detalhes
                _cliente = await _consultaCliente.Consultar(_clienteView.connection, _clienteView.GetCodigo());

                // Atualiza a visualização com os detalhes do cliente
                _clienteView.MostrarDetalhesCliente(_cliente);
                return;
            }

            FormsAdicionar?.Invoke(this, EventArgs.Empty);
        }

        public async void btnSalvar_Click(object sender, EventArgs e)
        {
            _cliente = _clienteView.ConstruirModel();

            await _updateCliente.Update(_clienteView.connection, _cliente, _clienteView.GetCodigo());

            _cliente = await _consultaCliente.Consultar(_clienteView.connection, _clienteView.GetCodigo());
            _clienteView.MostrarDetalhesCliente(_cliente);
        }

        public async void btnInserir_Click(object sender, EventArgs e)
        {
            _cliente = _clienteView.ConstruirModel();
            if (_cliente.ValidarIdade())
            {
                long novocodigo = await _inserirCliente.Inserir(_clienteView.connection, _cliente);

                _cliente = await _consultaCliente.Consultar(_clienteView.connection, Convert.ToString(novocodigo));
                _clienteView.MostrarDetalhesCliente(_cliente);
                return;
            }

            MessageBox.Show("Este cliente tem menos de 13 anos ou você não está preenchendo nenhum dado, não é possível cadastrar, por favor, verifique as informações inseridas", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public async void btnExcluir_Click(object sender, EventArgs e)
        {
            string codigo = _clienteView.GetCodigo();
            await _deleteCliente.Delete(_clienteView.connection, codigo);
            _deleteCliente.ShowSucessMessage($"Cliente {_cliente.Nome + " " + _cliente.Sobrenome}, excluido do Banco de dados com sucesso");

        }
        public void btnExibir_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = _exibirCliente.ExibirClientes(_clienteView.connection, _clienteView.GetBusca(), ds);

            _clienteView.dataGridClientes.DataSource = ds.Tables[0];

            foreach (DataGridViewColumn column in _clienteView.dataGridClientes.Columns)
            {
                column.ReadOnly = true;
            }
            _clienteView.dataGridClientes.Focus();
        }
    }

}
