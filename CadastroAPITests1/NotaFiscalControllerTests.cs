using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CadastroAPITests1.Tests
{
    [TestFixture]
    public class NotaFiscalControllerTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7185/");
            // Adicione qualquer configuração adicional necessária para o cliente HTTP
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [TestCaseSource(nameof(NotaFiscalTestData))]
        public async Task TestNotaFiscalAdd(string cnpj, string notaCupom, DateTime dataCompra, List<Produto> produtos)
        {
            var request = new
            {
                cnpj = cnpj,
                notaCupom = notaCupom,
                dataCompra = dataCompra,
                produtos = produtos
            };

            var response = await _client.PostAsJsonAsync("/api/NotaFiscal/add", request);
            response.EnsureSuccessStatusCode();
            // Adicione verificações adicionais conforme necessário
        }

        public static IEnumerable<TestCaseData> NotaFiscalTestData()
        {
            yield return new TestCaseData("27691475000128", "123456789", DateTime.Parse("2024-06-01"),
                new List<Produto> { new Produto { Nome = "fermento", Versao = "string", Quantidade = 3, Valor = 1.01M } });

            yield return new TestCaseData("27691475000128", "987654321", DateTime.Parse("2024-06-01"),
                new List<Produto> { new Produto { Nome = "fermento", Versao = "string", Quantidade = 2, Valor = 1.01M }, new Produto { Nome = "arroz", Versao = "integral", Quantidade = 1, Valor = 2.5M } });

            yield return new TestCaseData("27691475000128", "543216789", DateTime.Parse("2024-06-01"),
                new List<Produto> { new Produto { Nome = "fermento", Versao = "string", Quantidade = 1, Valor = 1.01M }, new Produto { Nome = "arroz", Versao = "integral", Quantidade = 2, Valor = 2.5M }, new Produto { Nome = "feijão", Versao = "carioca", Quantidade = 1, Valor = 3.0M } });

            // Mais testes aqui
        }
    }

    public class Produto
    {
        public string Nome { get; set; }
        public string Versao { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
