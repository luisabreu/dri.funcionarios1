using System;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;

namespace Domain.Handlers {
    public class Processador : IProcessador {
        public MsgGravacao Trata(CriaFuncionario comando) {
            throw new NotImplementedException();
        }

        public MsgGravacao Trata(ModificaContactosFuncionario comando) {
            throw new NotImplementedException();
        }

        public MsgGravacao Trata(ModificaDadosGeraisFuncionario comando) {
            throw new NotImplementedException();
        }
    }
}