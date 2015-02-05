using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Domain.Messages.Relatorios;
using NHibernate;
using NHibernate.Transform;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace Domain.Relatorios {
    public class GestorRelatoriosFuncionario : IGestorRelatoriosFuncionarios {
        private static Regex _nifRegex = new Regex(@"^\d{9}$");
        private readonly ISession _session;

        public GestorRelatoriosFuncionario(ISession session) {
            Contract.Requires(session != null);
            Contract.Ensures(_session != null);
            _session = session;
        }

        public IEnumerable<ResumoFuncionario> Pesquisa(string nomeOuNif) {
            const string sqlBase =
                "select Id, Nome, Nif, descricao as TipoFuncionario from Funcionarios f inner join TipoFuncinario tf on f.IdTipofuncionario=tf.Id ";
            var sql = sqlBase + (ENif(nomeOuNif) ? " where nif like '%:str%'" : " where nome like '%:str%'");
            var items = _session.CreateSQLQuery(sql)
                .SetString("str", nomeOuNif)
                .SetResultTransformer(Transformers.AliasToBean<ResumoFuncionario>())
                .List<ResumoFuncionario>();
            return items;
        }

        public FuncionarioDto Obtem(int idFuncionario) {
            return _session.Load<FuncionarioDto>(idFuncionario);
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