using System;

namespace CRUD_Clientes.Model
{
    public class ClienteModel
    {
        // Propriedades da classe ClienteModel
        public Int64 CodigoCliente { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Genero { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }

        // Método para validar a idade do cliente
        public bool ValidarIdade()
        {
            var resultado = CalcularIdade(DataNascimento);
            if (resultado <= 12)
            {
                return false; // Retorna falso se a idade for menor ou igual a 12
            }
            return true; // Retorna verdadeiro se a idade for maior que 12
        }

        // Método privado para calcular a idade com base na data de nascimento
        private int CalcularIdade(DateTime dataNascimento)
        {
            int idade = DateTime.Now.Year - dataNascimento.Year;
            if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
            {
                idade--; // Reduz a idade se o dia do ano atual for anterior ao dia do aniversário
            }
            return idade; // Retorna a idade calculada
        }
    }
}
