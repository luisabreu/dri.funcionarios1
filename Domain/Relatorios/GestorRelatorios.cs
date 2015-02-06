using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Domain.Messages;
using Domain.Messages.Relatorios;
using NHibernate;
using NHibernate.Transform;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace Domain.Relatorios {
    public class GestorRelatorios : IGestorRelatorios {
        private static Regex _nifRegex = new Regex(@"^\d{9}$");
        private readonly ISession _session;

        public GestorRelatorios(ISession session) {
            Contract.Requires(session != null);
            Contract.Ensures(_session != null);
            _session = session;
        }

        public IEnumerable<ResumoFuncionario> PesquisaFuncionarios(string nifOuNome) {
            const string sqlBase =
                "select Id, Nome, Nif, descricao as TipoFuncionario from Funcionarios f inner join TipoFuncinario tf on f.IdTipofuncionario=tf.Id ";
            var sql = sqlBase + (ENif(nifOuNome) ? " where nif like '%:str%'" : " where nome like '%:str%'");
            var items = _session.CreateSQLQuery(sql)
                .SetString("str", nifOuNome.Replace(' ', '%'))
                .SetResultTransformer(Transformers.AliasToBean<ResumoFuncionario>())
                .List<ResumoFuncionario>();
            return items;
        }

        public FuncionarioDto ObtemFuncionario(int idFuncionario) {
            return _session.Load<FuncionarioDto>(idFuncionario);
        }

        public IEnumerable<TipoFuncionario> ObtemTodosTiposFuncionarios() {
            return _session.QueryOver<TipoFuncionario>()
                .List<TipoFuncionario>();
        }

        private bool ENif(string nomeOuNif) {
            return _nifRegex.IsMatch(nomeOuNif);
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_session != null);
        }
    }
}