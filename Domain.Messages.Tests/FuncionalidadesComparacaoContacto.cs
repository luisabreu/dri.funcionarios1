using FluentAssertions;
using Xbehave;

namespace Domain.Messages.Tests {
    public class FuncionalidadesComparacaoContacto {
        [Scenario]
        public void Comparacao_deve_funcionar_com_dois_null(Telefone tel1, Telefone tel2, bool iguais) {
            "Quando recebe dois telefones a null"
                .Given(() => tel1 = tel2 = null);

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser true"
                .Then(() => iguais.Should().BeTrue());
        }
        
        [Scenario]
        public void Comparacao_deve_funcionar_com_um_null(Telefone tel1, Telefone tel2, bool iguais) {
            "Quando recebe um telefone válido e outro  null"
                .Given(() => tel1 = new Telefone("123123123"));

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser false"
                .Then(() => iguais.Should().BeFalse());
        }
        
        [Scenario]
        public void Comparacao_deve_funcionar_com_dois_telefones_iguais(Telefone tel1, Telefone tel2, bool iguais) {
            "Quando recebe um telefone válido"
                .Given(() => tel1 = new Telefone("123123123"));

            "E outro com o mesmo número"
                .And(() => tel2 = new Telefone("123123123"));

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser true"
                .Then(() => iguais.Should().BeTrue());
        }
        
        [Scenario]
        public void Comparacao_deve_funcionar_com_dois_telefones_diferentes(Telefone tel1, Telefone tel2, bool iguais) {
            "Quando recebe um telefone válido"
                .Given(() => tel1 = new Telefone("123123123"));

            "E outro com número diferente"
                .And(() => tel2 = new Telefone("222222222"));

            "E procedemos à comparação"
                .When(() => iguais = tel1 == tel2);

            "Então o resultado deve ser false"
                .Then(() => iguais.Should().BeFalse());
        }
        
        [Scenario]
        public void Dois_telefones_iguais_produzem_mesmo_hash(Telefone tel1, Telefone tel2, bool iguais) {
            "Quando recebe um telefone válido"
                .Given(() => tel1 = new Telefone("123123123"));

            "E outro com número diferente"
                .And(() => tel2 = new Telefone("123123123"));

            "E procedemos à comparação"
                .When(() => iguais = tel1.GetHashCode() == tel2.GetHashCode());

            "Então o resultado deve ser true"
                .Then(() => iguais.Should().BeTrue());
        }
    }
}