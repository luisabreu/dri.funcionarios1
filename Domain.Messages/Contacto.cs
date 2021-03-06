﻿using System;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Domain.Messages {
    public sealed class Contacto {
        private static readonly Regex _verificadorNumTel = new Regex(@"^\d{9}$");
        private static readonly Regex _verificadorNumExtensao = new Regex(@"^\d{4}$");
        private TipoContacto _tipoContacto;
        private string _valor;
        //NH
        private Contacto() : this(TipoContacto.Telefone, "123123123") {
        }

        private Contacto(TipoContacto tipoContacto = TipoContacto.Telefone, string valor = "123123123") {
            Contract.Requires(valor != null, Msg.Contacto_incorreto);
            _tipoContacto = tipoContacto;
            _valor = valor;
        }

        public TipoContacto TipoContacto {
            get { return _tipoContacto; }
        }

        public string Valor {
            get { return _valor; }
        }

        private bool Equals(Contacto other) {
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

        public static Contacto CriaTelefone(string numero) {
            Contract.Requires(numero != null, Msg.Contacto_incorreto);
            if (!_verificadorNumTel.IsMatch(numero)) {
                throw new InvalidOperationException(Msg.Contacto_incorreto);
            }
            return new Contacto(TipoContacto.Telefone, numero);
        }

        private static bool EmailValido(string valor) {
            try {
                new MailAddress(valor);
                return true;
            }
            catch {
                return false;
            }
        }

        public static Contacto CriaEmail(string mail) {
            Contract.Requires(mail != null, Msg.Contacto_incorreto);
            if (!EmailValido(mail)) {
                throw new InvalidOperationException(Msg.Contacto_incorreto);
            }
            return new Contacto(TipoContacto.Email, mail);
        }

        public static Contacto CriaExtensao(string ext) {
            Contract.Requires(ext != null, Msg.Contacto_incorreto);
            if (!_verificadorNumExtensao.IsMatch(ext)) {
                throw new InvalidOperationException(Msg.Contacto_incorreto);
            }
            return new Contacto(TipoContacto.Extensao, ext);
        }

        public static Contacto Parses(string contacto) {
            if (new Regex(@"^\d{9}$").IsMatch(contacto)) {
                return CriaTelefone(contacto);
            }
            if (new Regex(@"^\d{4}$").IsMatch(contacto)) {
                return CriaExtensao(contacto);
            }
            try {
                new MailAddress(contacto);
                return CriaEmail(contacto);
            }
            catch (Exception) {
                return null;
            }
        }
    }
}