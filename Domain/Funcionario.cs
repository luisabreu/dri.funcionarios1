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

        public Funcionario(CriaFuncionario funcionario) {
            Contract.Requires(funcionario != null);
            Contract.Ensures(!string.IsNullOrEmpty(_nome));
            Contract.Ensures(!string.IsNullOrEmpty(_nif));
            Contract.Ensures(_tipoFuncionario != null);
        }

        public void Executa(IComando cmd) {
            Contract.Requires(cmd != null);
        }
    }
}
