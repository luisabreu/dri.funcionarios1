using System.Configuration;
using GestorTransacoesBase = Domain.Mapeamentos.GestorTransacoes;

namespace site.Helpers {
    public class GestorTransacoes : GestorTransacoesBase {
        public override string ObtemCnnString() {
            return ConfigurationManager.ConnectionStrings["bd"].ConnectionString;
        }
    }
}