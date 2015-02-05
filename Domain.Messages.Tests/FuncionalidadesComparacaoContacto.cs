using FluentAssertions;
using Xbehave;

namespace Domain.Messages.Tests {
    public class FuncionalidadesComparacaoContacto {
        [Scenario]
        public void Comparacao_deve_funcionar_com_dois_null(Contacto tel1, Contacto tel2, bool iguais) {
            "Quando recebe dois Contactos a null"
                .Given(() => tel1 = tel2 = null);

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser true"
                .Then(() => iguais.Should().BeTrue());
        }
        
        [Scenario]
        public void Comparacao_deve_funcionar_com_um_null(Contacto tel1, Contacto tel2, bool iguais) {
            "Quando recebe um Contacto válido e outro  null"
                .Given(() => tel1 = Contacto.CriaTelefone("123123123"));

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser false"
                .Then(() => iguais.Should().BeFalse());
        }
        
        [Scenario]
        public void Comparacao_deve_funcionar_com_dois_Contactos_iguais(Contacto tel1, Contacto tel2, bool iguais) {
            "Quando recebe um Contacto válido"
                .Given(() => tel1 = Contacto.CriaTelefone("123123123"));

            "E outro com o mesmo número"
                .And(() => tel2 = Contacto.CriaTelefone("123123123"));

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser true"
                .Then(() => iguais.Should().BeTrue());
        }
        
        [Scenario]
        public void Comparacao_deve_funcionar_com_dois_Contactos_diferentes(Contacto tel1, Contacto tel2, bool iguais) {
            "Quando recebe um Contacto válido"
                .Given(() => tel1 = Contacto.CriaTelefone("123123123"));

            "E outro com número diferente"
                .And(() => tel2 = Contacto.CriaTelefone("222222222"));

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser false"
                .Then(() => iguais.Should().BeFalse());
        }
        
        [Scenario]
        public void Dois_Contactos_iguais_produzem_mesmo_hash(Contacto tel1, Contacto tel2, bool iguais) {
            "Quando recebe um Contacto válido"
                .Given(() => tel1 = Contacto.CriaTelefone("123123123"));

            "E outro com número diferente"
                .And(() => tel2 = Contacto.CriaTelefone("123123123"));

            "E procedemos à comparação"
                .When(() => iguais = tel1.GetHashCode() == tel2.GetHashCode());

            "Então o resultado deve ser true"
                .Then(() => iguais.Should().BeTrue());
        }
    }
}