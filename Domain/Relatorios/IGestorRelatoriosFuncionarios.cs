using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Domain.Messages.Relatorios;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace Domain.Relatorios {
    [ContractClass(typeof(ContratoGestorRelatoriosFuncionarios))]
    public interface IGestorRelatoriosFuncionarios {
        IEnumerable<ResumoFuncionario> Pesquisa(string nomeOuNif);
        FuncionarioDto Obtem(int idFuncionario);
    }

    [ContractClassFor(typeof(IGestorRelatoriosFuncionarios))]
    abstract class ContratoGestorRelatoriosFuncionarios:IGestorRelatoriosFuncionarios {
        public IEnumerable<ResumoFuncionario> Pesquisa(string nomeOuNif) {
            Contract.Requires(!string.IsNullOrEmpty(nomeOuNif));
            Contract.Ensures(Contract.Result<IEnumerable<ResumoFuncionario>>() != null);
            return default (IEnumerable<ResumoFuncionario>);
        }

        public FuncionarioDto Obtem(int idFuncionario) {
            Contract.Requires(idFuncionario > 0);
            return default(FuncionarioDto);
        }
    }
}