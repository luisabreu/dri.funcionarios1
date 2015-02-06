using System.Web.Mvc;
using Domain.Messages.Relatorios;
using Domain.Relatorios;
using FluentAssertions;
using Moq;
using NHibernate;
using site.Controllers;
using site.Models;
using Xbehave;

namespace site.tests.Controllers {
    public class FuncionalidadesFuncionariosController {
        private readonly AutoMockContainer _container = new AutoMockContainer(new MockRepository(MockBehavior.Strict));

        [Scenario]
        public void Interage_com_gerador_relatorios_para_pesquisar(FuncionariosController controlador, string pesquisa, ActionResult resultado) {
            var funcionarios = new[] {new ResumoFuncionario {Id = 1, Nif = "123456789", Nome = "Luis Abreu", TipoFuncionario = "Normal"}};
            "Dado um controlador"
                .Given(() => controlador = _container.Create<FuncionariosController>());

            "e uma string para pesquisa"
                .And(() => pesquisa = "Luis Abreu");

            "e alguns mocks"
                .And(() => {
                         var tran = _container.GetMock<ITransaction>();

                         _container.GetMock<ISession>()
                             .Setup(s => s.BeginTransaction())
                             .Returns(tran.Object);
                         tran.Setup(t => t.Dispose());

                         _container.GetMock<IGestorRelatoriosFuncionarios>()
                             .Setup(g => g.Pesquisa(pesquisa))
                             .Returns(funcionarios);
                     });

            "Quando efetuamos uma pesquisa"
                .When(() => resultado = controlador.Pesquisa(pesquisa));

            "Então devemos obter a vista e dados esperados"
                .Then(() => {
                          resultado.Should().BeAssignableTo<ViewResult>();
                          var model = ((ViewResult) resultado).Model as DadosPesquisa;
                          model.Funcionarios.Should().BeSameAs(funcionarios);
                          model.Pesquisa.Should().BeSameAs(pesquisa);
                      });

            "E mocks devem ter sido usados"
                .And(() => {
                         _container.GetMock<ISession>().VerifyAll();
                         _container.GetMock<ITransaction>().VerifyAll();
                     });
        }
    }
}