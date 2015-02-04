using System;
using System.Collections.Generic;
using Domain.Messages.Comandos;
using FluentAssertions;
using Xbehave;
using Xunit;

namespace Domain.Messages.Tests.Comandos {
    public class FuncionalidadesCriaFuncionario {
        private TipoFuncionario CriaFuncionarioParaTestes() {
            return new TipoFuncionario(1, "Professor");
        }

        [Scenario]
        public void Cria_comando_com_NIF_valido(Action criaComando, Exception excecaoEsperada) {
            "Uma operação de criação de comando com NIF válido"
                .Given(() => criaComando = () => new CriaFuncionario("luis", "123456789", CriaFuncionarioParaTestes()));

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
                .Given(() => criaComando = () => new CriaFuncionario("luis", "111111111", CriaFuncionarioParaTestes()));

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

        [Scenario]
        public void Cria_comando_sem_contactos(Action criaComando, Exception excecaoEsperada) {
            "Uma operação de criação de comando sem contactos"
                .Given(() => criaComando = () => new CriaFuncionario("luis", "123456789", CriaFuncionarioParaTestes()));

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
        public void Cria_comando_com_contactos_vazios(Action criaComando, Exception excecaoEsperada) {
            "Uma operação de criação de comando com coleção de contactos vazia"
                .Given(() => criaComando = () => new CriaFuncionario("luis", "123456789", CriaFuncionarioParaTestes(), new List<Contacto>()));

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
        public void Cria_comando_com_versao_positiva(Action criaComando, CriaFuncionario comando) {
            const int versao = 1;
            "Uma operação de criação de comando com uma versão positiva"
                .Given(() => criaComando = () => comando = new CriaFuncionario("luis", "123456789", CriaFuncionarioParaTestes(), null, 1));

            "Quando tentamos executar"
                .When(() =>criaComando() );

            "Então obtemos o campo version inicializado"
                .Then(() => comando.Versao.Should().Be(versao));
        }
        
    }
}