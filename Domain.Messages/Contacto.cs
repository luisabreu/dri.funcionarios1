using System;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Domain.Messages {
    public abstract class Contacto {
        private TipoContacto _tipoContacto;
        private string _valor;

        protected Contacto(TipoContacto tipoContacto = TipoContacto.Telefone, string valor = "123123123") {
            Contract.Requires(valor != null, Msg.Contacto_incorreto);
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
            return obj.GetType() == GetType() && Equals((Contacto) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((int) _tipoContacto*397) ^ (_valor != null ? _valor.GetHashCode() : 0);
            }
        }

        public static Telefone CriaTelefone(string numero) {
            return new Telefone(numero);
        }
        
        public static Email CriaEmail(string mail) {
            return new Email(mail);
        }
        
        public static Extensao CriaExtensao(string ext) {
            return new Extensao(ext);
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

    public class Email : Contacto {

        public Email(string valor) : base(TipoContacto.Email, valor) {
            Contract.Requires(valor != null, Msg.Contacto_incorreto);
            if (!IsValid(valor)) {
                throw new InvalidOperationException(Msg.Contacto_incorreto);
            }
        }

        private bool IsValid(string valor) {
            try {
                new MailAddress(valor);
                return true;
            }
            catch {
                return false;
            }
        }
    }

    public class Extensao : Contacto {
        private static readonly Regex _validator = new Regex(@"^\d{4}$");

        public Extensao(string valor) : base(TipoContacto.Email, valor) {
            Contract.Requires(valor != null, Msg.Contacto_incorreto);
            if (!IsValid(valor)) {
                throw new InvalidOperationException(Msg.Contacto_incorreto);
            }
        }

        private bool IsValid(string valor) {
            return _validator.IsMatch(valor);
        }
    }
}