using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using Domain.Messages;
using Domain.Messages.Comandos;

namespace Domain {
    public class Funcionario {
        private List<Contacto> _contactos;
        private int _id;
        private string _nif;
        private string _nome;
        private TipoFuncionario _tipoFuncionario;
        private int _version;

        private Funcionario() {
            //partial loading NH
            _nome = "NH";
            _nif = "123456789";
            _tipoFuncionario = new TipoFuncionario(1, "Something");
        }

        protected Funcionario(CriaFuncionario comando) {
            Contract.Requires(comando != null);
            Contract.Ensures(!string.IsNullOrEmpty(_nome));
            Contract.Ensures(!string.IsNullOrEmpty(_nif));
            Contract.Ensures(_tipoFuncionario != null);
            Contract.Ensures(_contactos != null);

            _nome = comando.Nome;
            _nif = comando.Nif;
            _tipoFuncionario = comando.TipoFuncionario;
            _contactos = new List<Contacto>(comando.Contactos ?? Enumerable.Empty<Contacto>());
            _version = comando.Versao;
        }

        //TODO: bad, more crud than cqrs...
        public int Id {
            get { return _id; }
        }

        public string Nome {
            get { return _nome; }
        }

        public string Nif {
            get { return _nif; }
        }

        public TipoFuncionario TipoFuncionario {
            get { return _tipoFuncionario; }
        }

        public IEnumerable<Contacto> Contactos {
            get { return _contactos; }
        }

        public int Version {
            get { return _version; }
        }

        public static Funcionario CriaNovo(CriaFuncionario cmd) {
            Contract.Requires(cmd != null);
            return new Funcionario(cmd);
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(!string.IsNullOrEmpty(_nome));
            Contract.Invariant(!string.IsNullOrEmpty(_nif));
            Contract.Invariant(_tipoFuncionario != null);
        }

        public void Modifica(ModificaDadosGeraisFuncionario comando) {
            Contract.Requires(comando != null);
            if (comando.Version != _version) {
                throw new InvalidOperationException(Msg.Objeto_modificado_por_outro_utilizador);
            }
            _nome = comando.Nome;
            _nif = comando.Nif;
            _tipoFuncionario = comando.TipoFuncionario;
        }

        public void Modifica(ModificaContactosFuncionario comando) {
            Contract.Requires(comando != null);
            if (comando.Versao != _version) {
                throw new InvalidOperationException(Msg.Objeto_modificado_por_outro_utilizador);
            }
            foreach (var contacto in comando.ContactosRemover) {
                _contactos.Remove(contacto);
            }
            foreach (var contacto in comando.ContactosAdicionar) {
                if (_contactos.Contains(contacto)) {
                    break;
                }
                _contactos.Add(contacto);
            }
        }
    }
}