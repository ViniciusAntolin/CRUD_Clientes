using System;

namespace CRUD_Clientes.Model
{
    public class ClienteModel
    {
        public Int64 CodigoCliente { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Genero { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }

        public bool ValidarIdade()
        {
            var resultado = CalcularIdade(DataNascimento);
            if (resultado <= 12)
            {
                return false;
            }
            return true;
        }

        private int CalcularIdade(DateTime datanasicmento)
        {
            int idade = DateTime.Now.Year - datanasicmento.Year;
            if (DateTime.Now.DayOfYear < datanasicmento.DayOfYear)
            {
                idade--;
            }
            return idade;
        }
    }
}
