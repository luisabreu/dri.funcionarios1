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
            const int totalRegistosDevolver = 1;

            "Dado um servico de verificação"
                .Given(() => servico = _container.Create<ServicoVerificacaoDuplicacaoNif>());

            "e um id"
                .And(() => id = 1);

            "e um nif"
                .And(() => nif = "123456789");

            "E alguns mocks"
                .And(() => PreparaMocks(id, nif, totalRegistosDevolver));

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
        
        
        [Scenario]
        public void Sem_duplicacao_quando_pesquisa_devolve_false(ServicoVerificacaoDuplicacaoNif servico, int id, string nif, bool resultado) {
            const int totalRegistosDevolver = 0;

            "Dado um servico de verificação"
                .Given(() => servico = _container.Create<ServicoVerificacaoDuplicacaoNif>());

            "e um id"
                .And(() => id = 1);

            "e um nif"
                .And(() => nif = "123456789");

            "E alguns mocks"
                .And(() => PreparaMocks(id, nif, totalRegistosDevolver));

            "Quando verificamos a duplicação"
                .When(() => resultado = servico.NifDuplicado(nif, id));

            "O resultado deve ser false"
                .Then(() => resultado.Should().BeFalse());

            "E servico deve ter sido usado corretamente"
                .And(() => {
                         _container.GetMock<ISession>().VerifyAll();
                         _container.GetMock<ISQLQuery>().VerifyAll();
                     });
        }

        private void PreparaMocks(int id, string nif, int totalADevolver) {
            var query = _container.GetMock<ISQLQuery>();
            query.Setup(q => q.SetString(It.IsAny<string>(), nif))
                .Returns(query.Object);
            query.Setup(q => q.SetInt32(It.IsAny<string>(), id))
                .Returns(query.Object);
            query.Setup(q => q.UniqueResult<int>())
                .Returns(totalADevolver);
            _container.GetMock<ISession>()
                .Setup(s => s.CreateSQLQuery(It.IsAny<string>()))
                .Returns(query.Object);
        }
    }
}