using System.Diagnostics.Contracts;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;

namespace Domain.Handlers {
    [ContractClass(typeof (TrataContracto))]
    public interface ITrata {
        MsgGravacao Trata(CriaFuncionario comando);
        MsgGravacao Trata(ModificaContactosFuncionario comando);
        MsgGravacao Trata(ModificaDadosGeraisFuncionario comando);
    }

    [ContractClassFor(typeof (ITrata))]
    internal abstract class TrataContracto : ITrata {
        public MsgGravacao Trata(CriaFuncionario comando) {
            Contract.Requires(comando != null);
            return default(MsgGravacao);
        }

        public MsgGravacao Trata(ModificaContactosFuncionario comando) {
            Contract.Requires(comando != null);
            return default(MsgGravacao);
        }

        public MsgGravacao Trata(ModificaDadosGeraisFuncionario comando) {
            Contract.Requires(comando != null);
            return default(MsgGravacao);
        }
    }
}