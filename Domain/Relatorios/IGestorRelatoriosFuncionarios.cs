using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Policy;
using Domain.Messages.Relatorios;

namespace Domain.Relatorios {
    [ContractClass(typeof(ContratoGestorRelatoriosFuncionarios))]
    public interface IGestorRelatoriosFuncionarios {
        IEnumerable<ResumoFuncionario> Pesquisa(string nomeOuNif);
    }

    [ContractClassFor(typeof(IGestorRelatoriosFuncionarios))]
    abstract class ContratoGestorRelatoriosFuncionarios:IGestorRelatoriosFuncionarios {
        public IEnumerable<ResumoFuncionario> Pesquisa(string nomeOuNif) {
            Contract.Requires(!string.IsNullOrEmpty(nomeOuNif));
            Contract.Ensures(Contract.Result<IEnumerable<ResumoFuncionario>>() != null);
            return default (IEnumerable<ResumoFuncionario>);
        }
    }
}