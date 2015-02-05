using Domain.Messages;
using FluentNHibernate.Testing;
using Xbehave;
using Xunit;

namespace Domain.Tests.Mapeamentos {
    public class MapeamentoTipoFuncionario {
        [Scenario]
        public void Mapeamento_tipo_funcionario_correto() {
            var tipoFuncionario = new TipoFuncionario(100, "Teste");
            new GestorTransacoesTestes().ExecutaTransacao((sess, tran) => {
                                                              new PersistenceSpecification<TipoFuncionario>(sess)
                                                                  .VerifyTheMappings(tipoFuncionario);
                                                                Assert.True(true);
                                                          });
        }
    }
}