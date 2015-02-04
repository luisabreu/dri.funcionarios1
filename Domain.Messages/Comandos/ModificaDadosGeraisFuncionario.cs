using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Domain.Messages.Comandos {
    public class ModificaDadosGeraisFuncionario : IComando {
        private readonly int _id;
        private readonly string _nif;
        private readonly string _nome;
        private readonly TipoFuncionario _tipoFuncionario;
        private readonly int _version;

        public ModificaDadosGeraisFuncionario(int id, int version, string nome, string nif, TipoFuncionario tipoFuncionario) {
            Contract.Requires(id > 0);
            Contract.Requires(version > 0);
            Contract.Requires(!string.IsNullOrEmpty(nome));
            Contract.Requires(!string.IsNullOrEmpty(nif));
            Contract.Requires(tipoFuncionario != null);
            Contract.Ensures(_id > 0);
            Contract.Ensures(_version > 0);
            Contract.Ensures(!string.IsNullOrEmpty(_nome));
            Contract.Ensures(!string.IsNullOrEmpty(_nif));
            Contract.Ensures(_tipoFuncionario != null);

            if (!VerificadorNif.NifValido(nif)) {
                throw new InvalidOperationException(Msg.Nif_invalido);
            }

            _id = id;
            _version = version;
            _nome = nome;
            _nif = nif;
            _tipoFuncionario = tipoFuncionario;
        }

        public int Id {
            get { return _id; }
        }

        public string Nif {
            get { return _nif; }
        }

        public string Nome {
            get { return _nome; }
        }

        public TipoFuncionario TipoFuncionario {
            get { return _tipoFuncionario; }
        }

        public int Version {
            get { return _version; }
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_id > 0);
            Contract.Invariant(_version > 0);
            Contract.Invariant(!string.IsNullOrEmpty(_nome));
            Contract.Invariant(!string.IsNullOrEmpty(_nif));
            Contract.Invariant(_tipoFuncionario != null);
        }
    }
}