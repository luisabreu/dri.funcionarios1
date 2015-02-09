using System;
using Domain.Messages.Handlers;

namespace site.Models {
    public static class MsgGravacaoExtensions {
        public static bool GravadaComSucesso(this MsgGravacao msg) {
            if (msg == null) {
                throw new NullReferenceException();
            }
            return msg.Id > 0 && msg.Versao > 0;
        }
    }
}