using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;
using Domain.Servicos;
using NHibernate;

namespace Domain.Handlers {
    public class Processador : IProcessador {
        private readonly ISession _session;
        private readonly IServicoVerificacaoDuplicacaoNif _verificadorDuplicacaoNif;

        public Processador(IServicoVerificacaoDuplicacaoNif verificadorDuplicacaoNif, ISession session) {
            Contract.Requires(verificadorDuplicacaoNif != null);
            Contract.Requires(session != null);
            Contract.Ensures(_verificadorDuplicacaoNif != null);
            Contract.Ensures(_session != null);
            _verificadorDuplicacaoNif = verificadorDuplicacaoNif;
            _session = session;
        }

        public MsgGravacao Trata(CriaFuncionario comando) {
            if (_verificadorDuplicacaoNif.NifDuplicado(comando.Nif, 0)) {
                throw new InvalidOperationException(Msg.Nif_duplicado);
            }
            var funcionario = Funcionario.CriaNovo(comando);
            Contract.Assert(funcionario != null, Msg.Erro_criar_funcionario_partir_comando);
            _session.Save(funcionario);
            _session.Flush();
            return new MsgGravacao {
                                       Id = funcionario.Id,
                                       Versao = funcionario.Versao
                                   };
        }

        public MsgGravacao Trata(ModificaContactosFuncionario comando) {
            throw new NotImplementedException();
        }

        public MsgGravacao Trata(ModificaDadosGeraisFuncionario comando) {
            throw new NotImplementedException();
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_verificadorDuplicacaoNif != null);
            Contract.Invariant(_session != null);
        }
    }
}