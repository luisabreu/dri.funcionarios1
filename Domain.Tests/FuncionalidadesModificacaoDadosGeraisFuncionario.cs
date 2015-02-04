using Domain.Messages;
using Domain.Messages.Comandos;
using FluentAssertions;
using Xbehave;

namespace Domain.Tests {
    public class FuncionalidadesModificacaoDadosGeraisFuncionario {
        [Scenario]
        public void Modifica_dados_gerais_do_funcionario(Funcionario funcionario, ModificaDadosGeraisFuncionario comando) {
            "Dado um funcionário"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), null, 1)));

            "E um comando de modificação de dados gerais"
                .And(() => comando = new ModificaDadosGeraisFuncionario(1, 1, "Luis Miguel", "215183517", new TipoFuncionario(2, "nORMAL")));

            "Quando aplicamos o comando"
                .When(() => funcionario.Modifica(comando));

            "Então devemos obter uma alteração de dados"
                .Then(() => {
                          funcionario.Nome.Should().Be(comando.Nome);
                          funcionario.Nif.Should().Be(comando.Nif);
                          funcionario.TipoFuncionario.Should().Be(comando.TipoFuncionario);
                      });
        }
    }
}