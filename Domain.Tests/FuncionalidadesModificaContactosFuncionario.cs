using System;
using Domain.Messages;
using Domain.Messages.Comandos;
using FluentAssertions;
using Xbehave;
using Xunit;

namespace Domain.Tests {
    public class FuncionalidadesModificaContactosFuncionario {
        [Scenario]
        public void Consegue_adicionar_novos_contactos(Funcionario funcionario, ModificaContactosFuncionario comando) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), null, 1)));

            "e um comando de modificação"
                .And(() => comando = new ModificaContactosFuncionario(1, 1, null, new Contacto[] {Contacto.CriaEmail("luisabreu@gmail.com"), Contacto.CriaTelefone("123123123")}));

            "Quando aplicamos o comando"
                .When(() => funcionario.Modifica(comando));

            "Então devemos obter os novos contactos na lista de contactos"
                .Then(() => funcionario.Contactos.Should().Contain(comando.ContactosAdicionar));
        }

        [Scenario]
        public void Consegue_adicionar_novos_contactos_aos_existentes(Funcionario funcionario, ModificaContactosFuncionario comando) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), new[] {Contacto.CriaExtensao("1234")}, 1)));

            "e um comando de modificação"
                .And(() => comando = new ModificaContactosFuncionario(1, 1, null, new Contacto[] {Contacto.CriaEmail("luisabreu@gmail.com"), Contacto.CriaTelefone("123123123")}));

            "Quando aplicamos o comando"
                .When(() => funcionario.Modifica(comando));

            "Então devemos obter os novos contactos na lista de contactos"
                .Then(() => funcionario.Contactos.Should().Contain(comando.ContactosAdicionar));
        }

        [Scenario]
        public void Nao_duplica_contactos(Funcionario funcionario, ModificaContactosFuncionario comando) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), new[] {Contacto.CriaTelefone("123123123")}, 1)));

            "e um comando de modificação"
                .And(() => comando = new ModificaContactosFuncionario(1, 1, null, new Contacto[] {Contacto.CriaEmail("luisabreu@gmail.com"), Contacto.CriaTelefone("123123123")}));

            "Quando aplicamos o comando"
                .When(() => funcionario.Modifica(comando));

            "Então devemos obter os novos contactos na lista de contactos"
                .Then(() => funcionario.Contactos.Should().HaveCount(2).And.Contain(comando.ContactosAdicionar));
        }

        [Scenario]
        public void Consegue_remover_contactos(Funcionario funcionario, ModificaContactosFuncionario comando) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), new[] {Contacto.CriaExtensao("1234")}, 1)));

            "e um comando de modificação"
                .And(() => comando = new ModificaContactosFuncionario(1, 1, new Contacto[] {Contacto.CriaExtensao("1234")}, null));

            "Quando aplicamos o comando"
                .When(() => funcionario.Modifica(comando));

            "Então devemos obter os novos contactos na lista de contactos"
                .Then(() => funcionario.Contactos.Should().NotContain(comando.ContactosRemover));
        }

        [Scenario]
        public void Funciona_corretamente_mesmo_com_contactos_inexistentes(Funcionario funcionario, ModificaContactosFuncionario comando) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), new[] {Contacto.CriaExtensao("1234")}, 1)));

            "e um comando de modificação"
                .And(() => comando = new ModificaContactosFuncionario(1, 1, new Contacto[] {Contacto.CriaEmail("luisabreu@gmail.com"), Contacto.CriaExtensao("1234")}, null));

            "Quando aplicamos o comando"
                .When(() => funcionario.Modifica(comando));

            "Então devemos obter os novos contactos na lista de contactos"
                .Then(() => funcionario.Contactos.Should().NotContain(comando.ContactosRemover));
        }
        
        [Scenario]
        public void Nao_funciona_com_versoes_diferentes(Funcionario funcionario, ModificaContactosFuncionario comando, Exception excecaoEsperada) {
            "Dado um funcionário existente"
                .Given(() => funcionario = Funcionario.CriaNovo(new CriaFuncionario("Luis", "123456789", new TipoFuncionario(1, "Docente"), new[] {Contacto.CriaExtensao("1234")})));

            "e um comando de modificação"
                .And(() => comando = new ModificaContactosFuncionario(1, 1, new Contacto[] {Contacto.CriaEmail("luisabreu@gmail.com"), Contacto.CriaExtensao("1234")}, null));

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