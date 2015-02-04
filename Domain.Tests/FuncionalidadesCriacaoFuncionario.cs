using Domain.Messages;
using Domain.Messages.Comandos;
using FluentAssertions;
using Xbehave;
using Xunit;

namespace Domain.Tests {
    public class FuncionalidadesCriacaoFuncionario {
        [Scenario]
        public void Cria_funcionario_a_partir_comando(CriaFuncionario comando, Funcionario funcionario) {
            "Quando recebemos um comando de criação"
                .Given(() => comando = new CriaFuncionario("luis", "123456789", new TipoFuncionario(1, "teste"), new[] {Contacto.CriaTelefone("123456789")}));

            "E aplicamos esse comando"
                .When(() => funcionario = Funcionario.CriaNovo(comando));

            "Então obtemos um funcionário válido"
                .Then(() => {
                        Assert.NotNull(funcionario);
                          funcionario.Nome.Should().Be(comando.Nome);
                          funcionario.Nif.Should().Be(comando.Nif);
                          funcionario.TipoFuncionario.Should().Be(comando.TipoFuncionario);
                          funcionario.Contactos.Should().BeEquivalentTo(comando.Contactos);
                      });

        }
    }
}