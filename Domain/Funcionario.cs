using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Messages;
using Domain.Messages.Comandos;

namespace Domain
{
    public class Funcionario {
        private int _id;
        private string _nome;
        private string _nif;
        private TipoFuncionario _tipoFuncionario;
        private IEnumerable<Contacto> _contactos;
        private int _version;

        protected Funcionario(CriaFuncionario comando) {
            Contract.Requires(comando != null);
            Contract.Ensures(!string.IsNullOrEmpty(_nome));
            Contract.Ensures(!string.IsNullOrEmpty(_nif));
            Contract.Ensures(_tipoFuncionario != null);
            Contract.Ensures(_contactos != null);
            
            _nome = comando.Nome;
            _nif = comando.Nif;
            _tipoFuncionario = comando.TipoFuncionario;
            _contactos = comando.Contactos ?? Enumerable.Empty<Contacto>();
        }

        public static Funcionario CriaNovo(CriaFuncionario cmd) {
            Contract.Requires(cmd != null);
            return new Funcionario(cmd);
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


        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(_nome));
            Contract.Invariant(!string.IsNullOrEmpty(_nif));
            Contract.Invariant(_tipoFuncionario != null);
        }

    }
}
