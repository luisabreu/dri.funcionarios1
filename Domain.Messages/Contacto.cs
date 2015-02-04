using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Domain.Messages {
    public abstract class Contacto {
        private TipoContacto _tipoContacto;
        private string _valor;

        protected Contacto(TipoContacto tipoContacto = TipoContacto.Telefone, string valor = "123123123") {
            _tipoContacto = tipoContacto;
            _valor = valor;
        }

        protected bool Equals(Contacto other) {
            return _tipoContacto == other._tipoContacto && string.Equals(_valor, other._valor);
        }

        public static bool operator ==(Contacto left, Contacto right) {
            return Equals(left, right);
        }

        public static bool operator !=(Contacto left, Contacto right) {
            return !Equals(left, right);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Contacto) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((int) _tipoContacto*397) ^ (_valor != null ? _valor.GetHashCode() : 0);
            }
        }
    }

    public class Telefone : Contacto {
        private static readonly Regex _validator = new Regex(@"^\d{9}$");
        public Telefone(string valor) : base(TipoContacto.Telefone, valor) {
            Contract.Requires(valor != null, Msg.Contacto_incorreto);
            if (!IsValid(valor)) {
                throw new InvalidOperationException(Msg.Contacto_incorreto);
            }
        }

        private bool IsValid(string valor) {
            return _validator.IsMatch(valor);
        }
    }
    
    public class Email: Contacto {
        public Email(string valor) : base(TipoContacto.Email, valor) {
        }
    }

    public class Extensao : Contacto {
        public Extensao(TipoContacto tipoContacto , string valor ) : base(TipoContacto.Email, valor) {
        }
    }

}