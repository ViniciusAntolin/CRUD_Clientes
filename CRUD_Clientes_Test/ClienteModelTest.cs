using CRUD_Clientes.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CRUD_Clientes_Test
{
    [TestClass]
    public class ClienteModelTest
    {
        [TestMethod]
        public void Deve_Retornar_Verdadeiro_Quando_DataNascimento_For_Maior_Que_12()
        {
            var model = new ClienteModel();

            model.DataNascimento = new DateTime(2001, 11, 14);

            var resultado = model.ValidarIdade();

            Assert.IsTrue(resultado);
        }
        [TestMethod]
        public void Deve_Retornar_Falso_Quando_DataNascimento_For_Menor_Ou_Igual_Que_12()
        {
            var model = new ClienteModel();

            model.DataNascimento = new DateTime(2011, 11, 14);

            var resultado = model.ValidarIdade();

            Assert.IsFalse(resultado);
        }
    }
}
