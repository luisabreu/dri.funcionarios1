using Domain.Handlers;
using Domain.Messages;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;
using Domain.Servicos;
using FluentAssertions;
using Moq;
using NHibernate;
using Xbehave;
using Xunit;

namespace Domain.Tests.Handlers {
    public class FuncionalidadesProcessador {
        private readonly AutoMockContainer _container = new AutoMockContainer(new MockRepository(MockBehavior.Strict));

        [Scenario]
        public void Trata_comando_criacao_com_sucesso(CriaFuncionario comando, Processador processador, MsgGravacao msg) {
            const int idEsperado = 1;
            const int versaoEsperado = 1;

            "Dado um comando de criação"
                .Given(() => comando = new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente")));

            "e uma handler"
                .And(() => processador = _container.Create<Processador>());

            "e alguns mocks"
                .And(() => {
                         _container.GetMock<IServicoVerificacaoDuplicacaoNif>()
                             .Setup(s => s.NifDuplicado(comando.Nif, 0))
                             .Returns(false);
                         var session = _container.GetMock<ISession>();
                         session.Setup(s => s.Save(It.IsAny<Funcionario>()))
                             .Returns(idEsperado);
                         session.Setup(s => s.Flush());
                     });

            "Quando tratamos o comando"
                .When(() => msg = processador.Trata(comando));

            "Então obtemos uma mensagem devidamente inicializada"
                .Then(() => Assert.NotNull(msg));

            "E os mocks foram usados corretamente"
                .And(() => {
                         _container.GetMock<IServicoVerificacaoDuplicacaoNif>().VerifyAll();
                         _container.GetMock<ISession>().VerifyAll();
                     });
        }
    }
}