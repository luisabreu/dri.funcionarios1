using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Domain.Messages {
    public sealed class TipoFuncionario {
        private readonly string _descricao;
        private readonly int _id;

        //NH
        private TipoFuncionario():this(0, "") {
            
        }
        public TipoFuncionario(int id = 0, string descricao = "") {
            Contract.Requires(id >= 0, Msg.Numero_superior_0);
            Contract.Requires(descricao != null, Msg.String_nao_pode_ser_nula);
            _id = id;
            _descricao = descricao;
        }


        //props only for mapping NH
        public int Id {
            get { return _id; }
            
        }

        public string Descricao{
            get { return _descricao; }
            
        }

        public override string ToString() {
            return _descricao;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_id >= 0, string.Format(Msg.Numero_superior_0, _id));
            Contract.Invariant(_descricao != null, string.Format(Msg.String_nao_pode_ser_nula, _descricao));
        }

        private bool Equals(TipoFuncionario other) {
            return string.Equals(_descricao, other._descricao) && _id == other._id;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is TipoFuncionario && Equals((TipoFuncionario) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((_descricao != null ? _descricao.GetHashCode() : 0)*397) ^ _id;
            }
        }

        public static bool operator ==(TipoFuncionario left, TipoFuncionario right) {
            return Equals(left, right);
        }

        public static bool operator !=(TipoFuncionario left, TipoFuncionario right) {
            return !Equals(left, right);
        }
    }
}