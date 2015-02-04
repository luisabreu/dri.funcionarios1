using System;
using Domain.Messages.Comandos;
using Xbehave;
using Xunit;

namespace Domain.Messages.Tests.Comandos {
    public class FuncionalidadesModificaDadosGeraisFuncionario {
        [Scenario]
        public void Cria_comando_com_NIF_valido(Action criaComando, Exception excecaoEsperada) {
            "Uma operação de criação de comando com NIF válido"
                .Given(() => criaComando = () => new ModificaDadosGeraisFuncionario(1, 1, "luis", "123456789", CriaFuncionarioParaTestes()));

            "Quando tentamos executar"
                .When(() => {
                          try {
                              criaComando();
                          }
                          catch (Exception ex) {
                              excecaoEsperada = ex;
                          }
                      });

            "Então não obtemos exceção"
                .Then(() => Assert.Null(excecaoEsperada));
        }

        [Scenario]
        public void Nao_cria_comando_com_NIF_invalido(Action criaComando, Exception excecaoEsperada) {
            "Uma operação de criação de comando com NIF válido"
                .Given(() => criaComando = () => new ModificaDadosGeraisFuncionario(1, 1, "luis", "111111111", CriaFuncionarioParaTestes()));

            "Quando tentamos executar"
                .When(() => {
                          try {
                              criaComando();
                          }
                          catch (Exception ex) {
                              excecaoEsperada = ex;
                          }
                      });

            "Então obtemos exceção"
                .Then(() => Assert.IsAssignableFrom<InvalidOperationException>(excecaoEsperada));
        }

        private TipoFuncionario CriaFuncionarioParaTestes() {
            return new TipoFuncionario(1, "Professor");
        }
    }
}