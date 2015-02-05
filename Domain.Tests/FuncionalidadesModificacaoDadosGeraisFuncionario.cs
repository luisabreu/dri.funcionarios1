using System;
using Domain.Messages;
using Domain.Messages.Comandos;
using FluentAssertions;
using Xbehave;
using Xunit;

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

        [Scenario]
        public void Nao_funciona_com_versoes_diferentes(Funcionario funcionario, ModificaDadosGeraisFuncionario comando, Exception excecaoEsperada) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), new[] {Contacto.CriaExtensao("1234")})));

            "e um comando de modificação"
                .And(() => comando = new ModificaDadosGeraisFuncionario(1, 2, "ll", "123456789", new TipoFuncionario(1, "Docente")));

            "Quando aplicamos o comando"
                .When(() => {
                          try {
                              funcionario.Modifica(comando);
                          }
                          catch (Exception ex) {
                              excecaoEsperada = ex;
                          }
                      });

            "Então devemos obter uma exceção"
                .Then(() => Assert.IsType<InvalidOperationException>(excecaoEsperada));
        }
    }
}