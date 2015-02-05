using System;
using Xbehave;
using Xunit;

namespace Domain.Messages.Tests {
    public class FuncionalidadesCriacaoEmail {
        [Scenario]
        public void Gera_erro_se_email_estiver_errado(Action criacao, Exception excecaoEsperada) {
            "Dada uma opera��o de cria��o de um email"
                .Given(() => criacao = () => Contacto.CriaEmail("123ddd123"));

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
        public void Cria_telefone_se_Email_estiver_certo(Action criacao, Exception excecaoEsperada) {
            "Dada uma opera��o de cria��o de um email"
                .Given(() => criacao = () => Contacto.CriaEmail("luisabure@gov-madeira.pt"));

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