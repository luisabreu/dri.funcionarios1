using System.Collections.Generic;
using Domain.Messages;
using FluentNHibernate.Testing;
using Xbehave;
using Xunit;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace Domain.Tests.Mapeamentos {
    public class MapeamentoFuncionarioDto {
        [Scenario]
        public void Mapeamento_funcionario_correto() {
            var tipoFuncionario = new TipoFuncionario(100, "Teste");
            var funcionario = new FuncionarioDto {
                Id = 1,
                Contactos = new List<Contacto> {Contacto.CriaExtensao("1234")},
                Nif = "123456789",
                Nome = "Luis",
                TipoFuncionario = tipoFuncionario
            };
            new GestorTransacoesTestes().ExecutaTransacao((sess, tran) => {
                sess.Save(tipoFuncionario);
                sess.Flush();
                new PersistenceSpecification<FuncionarioDto>(sess)
                    .VerifyTheMappings(funcionario);
                Assert.True(true);
            });
        }
    }
}