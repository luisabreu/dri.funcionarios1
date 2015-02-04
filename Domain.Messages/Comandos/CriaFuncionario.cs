using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Domain.Messages.Comandos {
    public class CriaFuncionario : IComando {
        private readonly IEnumerable<Contacto> _contactos;
        private readonly string _nif;
        private readonly string _nome;
        private readonly TipoFuncionario _tipoFuncionario;
        private readonly int _versao;

        public CriaFuncionario(string nome, string nif, TipoFuncionario tipoFuncionario, IEnumerable<Contacto> contactos = null)
            : this(nome, nif, tipoFuncionario, contactos, 0) {
        }

        internal CriaFuncionario(string nome, string nif, TipoFuncionario tipoFuncionario, IEnumerable<Contacto> contactos, int versao) {
            Contract.Requires(!string.IsNullOrEmpty(nome), Msg.String_nao_pode_ser_nula);
            Contract.Requires(!string.IsNullOrEmpty(nif), Msg.String_nao_pode_ser_nula);
            Contract.Requires(tipoFuncionario != null, Msg.Tipo_funcionario_tem_de_ser_definido);
            Contract.Requires(versao >= 0, Msg.Numero_superior_0);
            Contract.Ensures(!string.IsNullOrEmpty(_nome), Msg.String_nao_pode_ser_nula);
            Contract.Ensures(!string.IsNullOrEmpty(_nif), Msg.String_nao_pode_ser_nula);
            Contract.Ensures(_tipoFuncionario != null, Msg.Tipo_funcionario_tem_de_ser_definido);
            Contract.Ensures(_versao >= 0, Msg.Numero_superior_0);
            if (!VerificadorNif.NifValido(nif)) {
                throw new InvalidOperationException(Msg.Nif_invalido);
            }
            _nome = nome;
            _nif = nif;
            _tipoFuncionario = tipoFuncionario;
            _contactos = contactos;
            _versao = versao;
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

        internal int Versao {
            get { return _versao; }
        }

        public IEnumerable<Contacto> Contactos {
            get { return _contactos; }
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(!string.IsNullOrEmpty(_nome), Msg.String_nao_pode_ser_nula);
            Contract.Invariant(!string.IsNullOrEmpty(_nif), Msg.String_nao_pode_ser_nula);
            Contract.Invariant(_tipoFuncionario != null, Msg.Tipo_funcionario_tem_de_ser_definido);
            Contract.Invariant(_versao >= 0, Msg.Numero_superior_0);
        }
    }
}