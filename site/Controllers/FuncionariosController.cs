using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using Domain.Relatorios;
using NHibernate;
using site.Models;

namespace site.Controllers {
    public class FuncionariosController : Controller {
        private readonly ISession _session;
        private readonly IGestorRelatoriosFuncionarios _gestorRelatorios;

        public FuncionariosController(ISession session, IGestorRelatoriosFuncionarios gestorRelatorios) {
            Contract.Requires(session != null);
            Contract.Requires(gestorRelatorios != null);
            Contract.Ensures(_session != null);
            Contract.Ensures(_gestorRelatorios != null);
            _session = session;
            _gestorRelatorios = gestorRelatorios;
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult Pesquisa(string nifOuNome) {
            Contract.Requires(!string.IsNullOrEmpty(nifOuNome), Msg.String_vazia);
            using (var tran = _session.BeginTransaction()) {
                var funcionarios = _gestorRelatorios.Pesquisa(nifOuNome);
                return View("Index", new DadosPesquisa {Pesquisa = nifOuNome, Funcionarios = funcionarios});
            }
        }

        public ActionResult Funcionario(string nifOuNome) {
            return View();
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_session != null);
        }
    }
}