using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Domain.Messages {
    public sealed class TipoFuncionario {
        private readonly string _descricao;
        private readonly int _idTipoFuncionario;

        public TipoFuncionario(int idTipoFuncionario = 0, string descricao = "") {
            Contract.Requires(idTipoFuncionario >= 0, Msg.Numero_superior_0);
            Contract.Requires(descricao != null, Msg.String_nao_pode_ser_nula);
            _idTipoFuncionario = idTipoFuncionario;
            _descricao = descricao;
        }

        public override string ToString() {
            return _descricao;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_idTipoFuncionario >= 0, string.Format(Msg.Numero_superior_0, _idTipoFuncionario));
            Contract.Invariant(_descricao != null, string.Format(Msg.String_nao_pode_ser_nula, _descricao));
        }
    }
}