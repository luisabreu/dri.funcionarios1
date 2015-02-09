using System.Net.Http;
using System.Web.Routing;
using MvcRouteTester;
using site.Controllers;
using Xunit;

namespace site.tests {
    public class RouteTests {
        private readonly RouteCollection _rotas = new RouteCollection();

        public RouteTests() {
            RouteConfig.RegisterRoutes(_rotas);
        }

        [Fact]
        public void Verifica_url_base() {
            _rotas.ShouldMap("/").To<FuncionariosController>(HttpMethod.Get, h => h.Index());
        }

        [Fact]
        public void Verifica_url_pesquisa_funcionarios() {
            _rotas.ShouldMap("/pesquisa/Luis Abreu").To<FuncionariosController>(HttpMethod.Get, h => h.Pesquisa("Luis Abreu"));
        }

        [Fact]
        public void Verifica_url_ficha_individual_funcionario() {
            _rotas.ShouldMap("/funcionario?id=1").To<FuncionariosController>(h => h.Funcionario(1));
        }
    }
}