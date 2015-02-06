using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using Domain.Relatorios;
using NHibernate;
using site.Models;
using FuncionarioDto=Domain.Messages.Relatorios.Funcionario;

namespace site.Controllers {
    public class FuncionariosController : Controller {
        private readonly ISession _session;
        private readonly IGestorRelatorios _gestorRelatorios;

        public FuncionariosController(ISession session, IGestorRelatorios gestorRelatorios) {
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
                var funcionarios = _gestorRelatorios.PesquisaFuncionarios(nifOuNome);
                return View("Index", new DadosPesquisa {Pesquisa = nifOuNome, Funcionarios = funcionarios});
            }
        }

        public ActionResult Funcionario(int nifOuNome) {
            Contract.Requires(nifOuNome > 0, Msg.Numero_tem_ser_positivo);
            using (var tran = _session.BeginTransaction()) {
                var func = _gestorRelatorios.ObtemFuncionario(nifOuNome);
                Contract.Assert(func != null, Msg.Funcionario_inexistente);
                var tipos = _gestorRelatorios.ObtemTodosTiposFuncionarios();

                return View(new DadosFormularioFuncionario {Funcionario = func, TiposFuncionario = tipos});
            }
            
        }
        
        
        public ActionResult Funcionario(FuncionarioDto funcionario) {
            
            throw new NotImplementedException();
        }

        private static bool Novo(FuncionarioDto funcionario) {
            return funcionario != null && funcionario.Id == 0 && funcionario.Versao == 0;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant() {
            Contract.Invariant(_session != null);
        }
    }
}