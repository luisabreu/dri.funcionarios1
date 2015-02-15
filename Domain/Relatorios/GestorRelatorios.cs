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
                "select f.Id, Nome, Nif, descricao as TipoFuncionario from Funcionarios f inner join TipoFuncionario tf on f.IdTipofuncionario=tf.Id where {0} like :str";
            var sql = string.Format(sqlBase,  (ENif(nifOuNome) ? " nif " : " nome "));
            var items = _session.CreateSQLQuery(sql)
                .SetString("str", "%" + nifOuNome.Replace(' ', '%') + "%")
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

        public TipoFuncionario ObtemTipoFuncionario(int idTipoFuncionario) {
            return _session.Load<TipoFuncionario>(idTipoFuncionario);
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