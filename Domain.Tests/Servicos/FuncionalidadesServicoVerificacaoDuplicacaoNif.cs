using Domain.Servicos;
using FluentAssertions;
using Moq;
using NHibernate;
using Xbehave;

namespace Domain.Tests.Servicos {
    public class FuncionalidadesServicoVerificacaoDuplicacaoNif {
        private readonly AutoMockContainer _container = new AutoMockContainer(new MockRepository(MockBehavior.Strict));

        [Scenario]
        public void Indica_duplicacao_quando_pesquisa_devolve_true(ServicoVerificacaoDuplicacaoNif servico, int id, string nif, bool resultado) {
           

            "Dado um servico de verificação"
                .Given(() => servico = _container.Create<ServicoVerificacaoDuplicacaoNif>());

            "e um id"
                .And(() => id = 1);

            "e um nif"
                .And(() => nif = "123456789");

            "E alguns mocks"
                .And(() => {
                    var query = _container.GetMock<ISQLQuery>();
                    query.Setup(q => q.SetString(It.IsAny<string>(), nif))
                        .Returns(query.Object);
                    query.Setup(q => q.SetInt32(It.IsAny<string>(), id))
                        .Returns(query.Object);
                    query.Setup(q => q.UniqueResult<int>())
                        .Returns(1);
                    _container.GetMock<ISession>()
                        .Setup(s => s.CreateSQLQuery(It.IsAny<string>()))
                        .Returns(query.Object);

                     });

            "Quando verificamos a duplicação"
                .When(() => resultado = servico.NifDuplicado(nif, id));

            "O resultado deve ser true"
                .Then(() => resultado.Should().BeTrue());

            "E servico deve ter sido usado corretamente"
                .And(() => {
                         _container.GetMock<ISession>().VerifyAll();
                         _container.GetMock<ISQLQuery>().VerifyAll();
                     });
        }
    }
}