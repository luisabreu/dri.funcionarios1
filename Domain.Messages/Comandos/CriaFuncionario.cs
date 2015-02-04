using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain.Messages.Comandos {
    public class CriaFuncionario : IComando {
        private readonly IEnumerable<Contacto> _contactos;
        private readonly string _nif;
        private readonly string _nome;

        public CriaFuncionario(string nome, string nif, IEnumerable<Contacto> contactos = null) {
            Contract.Requires(!string.IsNullOrEmpty(nome), Msg.String_nao_pode_ser_nula);
            Contract.Requires(!string.IsNullOrEmpty(nif), Msg.String_nao_pode_ser_nula);
            if (!VerificadorNif.NifValido(nif)) {
                throw new InvalidOperationException(Msg.Nif_invalido);
            }
            _nome = nome;
            _nif = nif;
            _contactos = contactos;
        }

        public string Nome {
            get { return _nome; }
        }

        public string Nif {
            get { return _nif; }
        }

        public IEnumerable<Contacto> Contactos {    
            get { return _contactos; }
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(!string.IsNullOrEmpty(_nome), Msg.String_nao_pode_ser_nula);
            Contract.Invariant(!string.IsNullOrEmpty(_nif), Msg.String_nao_pode_ser_nula);
        }

        [ContractVerification(false)]
        private static class VerificadorNif {
            private static readonly Regex _baseNifReg = new Regex("^\\d{9}$");

            public static bool NifValido(string nif) {
                Contract.Requires(!String.IsNullOrEmpty(nif));

                if (!_baseNifReg.IsMatch(nif)) {
                    return false;
                }

                if (!FirstNumberIsCorrect(nif)) return false;
                if (nif == null || nif.Length != 9) {
                    return false;
                }
                return CalculateCheckDigit(nif) == GetIntFromChar(nif[8]);
            }

            private static bool FirstNumberIsCorrect(String nif) {
                var validFirstChars = new[] {'1', '2', '5', '6', '7', '8', '9'};
                return validFirstChars.Contains(nif[0]);
            }

            [ContractVerification(false)]
            private static int CalculateCheckDigit(string nif) {
                var checkDigit = 0;
                for (var i = 0; i < 8; i++) {
                    checkDigit += GetIntFromChar(nif[i])*(9 - i);
                }

                checkDigit = 11 - (checkDigit%11);
                if (checkDigit >= 10) {
                    checkDigit = 0;
                }
                return checkDigit;
            }

            private static Int32 GetIntFromChar(char currentChar) {
                return Convert.ToInt32(char.GetNumericValue(currentChar));
            }
        }
    }
}
