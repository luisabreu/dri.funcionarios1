using System.Diagnostics.Contracts;
using NHibernate.Mapping;

namespace Domain.Servicos {
    [ContractClass(typeof (ContratoServicoVerificacaoDuplicacaoNif))]
    public interface IServicoVerificacaoDuplicacaoNif {
        bool NifDuplicado(string nif, int id);
    }

    [ContractClassFor(typeof (IServicoVerificacaoDuplicacaoNif))]
    internal abstract class ContratoServicoVerificacaoDuplicacaoNif : IServicoVerificacaoDuplicacaoNif {
        public bool NifDuplicado(string nif, int id) {
            Contract.Requires(!string.IsNullOrEmpty(nif), Msg.String_vazia);
            Contract.Requires(id > 0);
            return default (bool);
        }
    }
}