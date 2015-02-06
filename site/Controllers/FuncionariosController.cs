using System.Web.Mvc;

namespace site.Controllers {
    public class FuncionariosController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Pesquisa(string nifOuNome) {
            return View();
        }

        public ActionResult Funcionario(string nifOuNome) {
            return View();
        }
    }
}