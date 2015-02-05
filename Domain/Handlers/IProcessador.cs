using System.Diagnostics.Contracts;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;

namespace Domain.Handlers {
    [ContractClass(typeof (ProcessadorContracto))]
    public interface IProcessador {
        MsgGravacao Trata(CriaFuncionario comando);
        MsgGravacao Trata(ModificaContactosFuncionario comando);
        MsgGravacao Trata(ModificaDadosGeraisFuncionario comando);
    }

    [ContractClassFor(typeof (IProcessador))]
    internal abstract class ProcessadorContracto : IProcessador {
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