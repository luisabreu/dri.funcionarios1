﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Domain.Messages.Comandos {
    public class ModificaContactosFuncionario {
        private readonly IEnumerable<Contacto> _contactosAdicionar;
        private readonly IEnumerable<Contacto> _contactosRemover;
        private readonly int _id;
        private readonly int _versao;

        public ModificaContactosFuncionario(int id, int versao, IEnumerable<Contacto> contactosRemover, IEnumerable<Contacto> contactosAdicionar) {
            Contract.Requires(id > 0, Msg.Numero_superior_0);
            Contract.Requires(versao > 0, Msg.Numero_superior_0);
            Contract.Requires(contactosRemover != null || contactosAdicionar != null);
            Contract.Ensures(_id > 0);
            Contract.Ensures(_versao > 0);
            Contract.Ensures(_contactosAdicionar != null);
            Contract.Ensures(_contactosRemover != null);

            _id = id;
            _versao = versao;
            _contactosRemover = contactosRemover ?? Enumerable.Empty<Contacto>();
            _contactosAdicionar = contactosAdicionar ?? Enumerable.Empty<Contacto>();
        }

        public IEnumerable<Contacto> ContactosAdicionar {
            get { return _contactosAdicionar; }
        }

        public IEnumerable<Contacto> ContactosRemover {
            get { return _contactosRemover; }
        }

        public int Id {
            get { return _id; }
        }

        public int Versao {
            get { return _versao; }
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_id > 0);
            Contract.Invariant(_versao > 0);
            Contract.Invariant(_contactosAdicionar != null);
            Contract.Invariant(_contactosRemover != null);
        }
    }
}