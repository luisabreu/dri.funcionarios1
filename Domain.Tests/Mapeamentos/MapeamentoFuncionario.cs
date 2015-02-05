using Domain.Messages;
using Domain.Messages.Comandos;
using FluentNHibernate.Testing;
using Xbehave;
using Xunit;

namespace Domain.Tests.Mapeamentos {
    public class MapeamentoFuncionario {
        [Scenario]
        public void Mapeamento_funcionario_correto() {
            var tipoFuncionario = new TipoFuncionario(100, "Teste");
            var funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", tipoFuncionario, new[] {Contacto.CriaTelefone("123456789")}));
            new GestorTransacoesTestes().ExecutaTransacao((sess, tran) => {
                                                              sess.Save(tipoFuncionario);
                                                              sess.Flush();
                                                              new PersistenceSpecification<Funcionario>(sess)
                                                                  .VerifyTheMappings(funcionario);
                                                              Assert.True(true);
                                                          });
        }
    }
}