using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using Domain.Handlers;
using Domain.Messages;
using Domain.Messages.Comandos;
using Domain.Messages.Handlers;
using Domain.Messages.Relatorios;
using Domain.Relatorios;
using NHibernate;
using site.Models;
using FuncionarioDto=Domain.Messages.Relatorios.Funcionario;

namespace site.Controllers {
    public class FuncionariosController : Controller {
        private readonly ISession _session;
        private readonly IGestorRelatorios _gestorRelatorios;
        private readonly IProcessador _processador;

        public FuncionariosController(ISession session, IGestorRelatorios gestorRelatorios, IProcessador processador) {
            Contract.Requires(session != null);
            Contract.Requires(gestorRelatorios != null);
            Contract.Requires(processador != null);
            Contract.Ensures(_session != null);
            Contract.Ensures(_gestorRelatorios != null);
            Contract.Ensures(_processador != null);
            _session = session;
            _gestorRelatorios = gestorRelatorios;
            _processador = processador;
        }

        public ActionResult Index() {
            return View(new DadosPesquisa{NifOuNome = "", Funcionarios = Enumerable.Empty<ResumoFuncionario>(), PesquisaEfetuada = false});
        }

        public ActionResult Pesquisa(string nifOuNome) {
            Contract.Requires(!string.IsNullOrEmpty(nifOuNome), Msg.String_vazia);
            using (var tran = _session.BeginTransaction()) {
                var funcionarios = _gestorRelatorios.PesquisaFuncionarios(nifOuNome);
                return View("Index", new DadosPesquisa { NifOuNome = nifOuNome, Funcionarios = funcionarios, PesquisaEfetuada = true });
            }
        }

        public ActionResult Funcionario(int? nifOuNome) {
            using (var tran = _session.BeginTransaction()) {
                var func = nifOuNome.HasValue ? _gestorRelatorios.ObtemFuncionario(nifOuNome.Value) : null;
                var tipos = _gestorRelatorios.ObtemTodosTiposFuncionarios();

                return View(new DadosFormularioFuncionario {Funcionario = func, TiposFuncionario = tipos, Novo = func == null || Novo(func)});
            }
            
        }

        public ActionResult ModificaDadosGerais() {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult CriaNovoFuncionario(NovoFuncionario novo) {
            using (var tran = _session.BeginTransaction()) {
                var comando = new CriaFuncionario(novo.Nome, novo.Nif, _gestorRelatorios.ObtemTipoFuncionario(novo.TipoFuncionario));
                var msg = _processador.Trata(comando);
                tran.Commit();
                if (msg != null && msg.Id != 0 && msg.Versao != 0) {
                    return RedirectToAction("Funcionario", new { nifOuNome = msg.Id });
                }
            }
            
            return View("Funcionario");
        }

        private static bool Novo(FuncionarioDto funcionario) {
            return funcionario != null && funcionario.Id == 0 && funcionario.Versao == 0;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_session != null);
            Contract.Invariant(_gestorRelatorios != null);
            Contract.Invariant(_processador != null);
        }
    }
}