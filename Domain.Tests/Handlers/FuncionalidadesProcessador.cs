using Domain.Handlers;
using Domain.Messages;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;
using FluentAssertions;
using Xbehave;

namespace Domain.Tests.Handlers {
    public class FuncionalidadesProcessador {
        [Scenario]
        public void Trata_comando_criacao(CriaFuncionario comando, Processador processador, MsgGravacao msg) {
            const int idEsperado = 1;
            const int versaoEsperado = 1;

            "Dado um comando de criação"
                .Given(() => comando = new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente")));

            "e uma handler"
                .And(() => processador = new Processador());

            "Quando tratamos o comando"
                .When(() => msg = processador.Trata(comando));

            "Então obtemos uma mensagem devidamente inicializada"
                .Then(() => {
                          msg.Id.Should().Be(idEsperado);
                          msg.Versao.Should().Be(versaoEsperado);
                      });
        }
    }
}