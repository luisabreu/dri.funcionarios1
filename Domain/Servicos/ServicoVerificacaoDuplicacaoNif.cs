using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using NHibernate;

namespace Domain.Servicos {
    public class ServicoVerificacaoDuplicacaoNif : IServicoVerificacaoDuplicacaoNif {
        private readonly ISession _session;

        public ServicoVerificacaoDuplicacaoNif(ISession session) {
            Contract.Requires(session != null);
            Contract.Ensures(_session != null);
            _session = session;
        }

        public bool NifDuplicado(string nif, int id) {
            const string sql = "select count(id) from funcionarios where nif =:nif and id<>:id";
            var total = _session.CreateSQLQuery(sql)
                .SetString("nif", nif)
                .SetInt32("id", id)
                .UniqueResult<int>();
            return total > 0;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_session != null);
        }
    }
}