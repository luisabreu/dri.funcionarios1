using System;
using Xbehave;
using Xunit;

namespace Domain.Messages.Tests {
    public class FuncionalidadesCriacaoExtensao {
        [Scenario]
        public void Gera_erro_se_numero_extensao_tiver_letras(Action criacao, Exception excecaoEsperada) {
            "Dada uma opera��o de cria��o de uma extens�o"
                .Given(() => criacao = () => Contacto.CriaExtensao("123d"));

            "Quando a executarmos"
                .When(() => {
                          try {
                              criacao();
                          }
                          catch (Exception e) {
                              excecaoEsperada = e;
                          }
                      });

            "A sua execu��o deve gerar erro"
                .Then(() => Assert.NotNull(excecaoEsperada));
        }
        
        [Scenario]
        public void Gera_erro_se_numero_extensao_nao_tiver_numero_esperado_digitos(Action criacao, Exception excecaoEsperada) {
            "Dada uma opera��o de cria��o de uma extens�o"
                .Given(() => criacao = () => Contacto.CriaExtensao("123"));

            "Quando a executarmos"
                .When(() => {
                          try {
                              criacao();
                          }
                          catch (Exception e) {
                              excecaoEsperada = e;
                          }
                      });

            "A sua execu��o deve gerar erro"
                .Then(() => Assert.NotNull(excecaoEsperada));
        }
        
        [Scenario]
        public void Cria_telefone_se_numero_tiver_4_digitos(Action criacao, Exception excecaoEsperada) {
            "Dada uma opera��o de cria��o de uma extens�o"
                .Given(() => criacao = () => Contacto.CriaExtensao("1234"));

            "Quando a executarmos"
                .When(() => {
                          try {
                              criacao();
                          }
                          catch (Exception e) {
                              excecaoEsperada = e;
                          }
                      });

            "A sua execu��o deve gerar erro"
                .Then(() => Assert.Null(excecaoEsperada));
        }
    }
}