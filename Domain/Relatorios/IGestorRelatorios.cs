using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Domain.Messages;
using Domain.Messages.Relatorios;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace Domain.Relatorios {
    [ContractClass(typeof(ContratoGestorRelatorios))]
    public interface IGestorRelatorios {
        IEnumerable<ResumoFuncionario> PesquisaFuncionarios(string nifOuNome);
        FuncionarioDto ObtemFuncionario(int idFuncionario);
        IEnumerable<TipoFuncionario> ObtemTodosTiposFuncionarios();
    }

    [ContractClassFor(typeof(IGestorRelatorios))]
    abstract class ContratoGestorRelatorios:IGestorRelatorios {
        public IEnumerable<ResumoFuncionario> PesquisaFuncionarios(string nifOuNome) {
            Contract.Requires(!string.IsNullOrEmpty(nifOuNome));
            Contract.Ensures(Contract.Result<IEnumerable<ResumoFuncionario>>() != null);
            return default (IEnumerable<ResumoFuncionario>);
        }

        public FuncionarioDto ObtemFuncionario(int idFuncionario) {
            Contract.Requires(idFuncionario > 0);
            return default(FuncionarioDto);
        }

        public IEnumerable<TipoFuncionario> ObtemTodosTiposFuncionarios() {
            return default(IEnumerable<TipoFuncionario>);
        }
    }
}