using System;
using System.Runtime.InteropServices;
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
        
        [Scenario]
        public void Trata_comando_criacao_com_duplicacao(CriaFuncionario comando, Processador processador, Exception excecaoEsperada ) {

            "Dado um comando de criação com nif duplicado"
                .Given(() => comando = new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente")));

            "e uma handler"
                .And(() => processador = _container.Create<Processador>());

            "e alguns mocks"
                .And(() => {
                         _container.GetMock<IServicoVerificacaoDuplicacaoNif>()
                             .Setup(s => s.NifDuplicado(comando.Nif, 0))
                             .Returns(true);
                     });

            "Quando tratamos o comando"
                .When(() => {
                          try {
                              processador.Trata(comando);
                          }
                          catch (Exception ex) {
                              excecaoEsperada = ex;
                          }
                      });

            "Então obtemos uma exceção"
                .Then(() => Assert.IsAssignableFrom<InvalidOperationException>(excecaoEsperada));

            "E os mocks foram usados corretamente"
                .And(() => {
                         _container.GetMock<IServicoVerificacaoDuplicacaoNif>().VerifyAll();
                         _container.GetMock<ISession>().VerifyAll();
                     });
        }


        public void Trata_comando_alteracao_contactos_com_sucesso(ModificaContactosFuncionario comando, Funcionario funcionario, Processador processador, MsgGravacao msg) {
            "Dado um comando de ateração de contactos"
                .Given(() => comando = new ModificaContactosFuncionario(0, 0, null, new[] {Contacto.CriaExtensao("1234")}));

            "um funcionário"
                .And(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"))));

            "e um processador"
                .And(() => processador = _container.Create<Processador>());

            "E alguns mocks"
                .And(() => {
                         var session = _container.GetMock<ISession>();
                         session.Setup(s => s.Load<Funcionario>(comando.Id))
                             .Returns(funcionario);
                         session.Setup(s => s.Update(funcionario));
                         session.Setup(s => s.Flush());
                     });

            "Quando tratamos o comando"
                .When(() => processador.Trata(comando));

            "Então obtemos uma mensagem com informação"
                .Then(() => msg.Id.Should().Be(comando.Id));

            "E os mocks foram usados"
                .And(() => _container.GetMock<ISession>().VerifyAll());
        }
    }
}