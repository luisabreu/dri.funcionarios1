using Domain.Mapeamentos;

namespace Domain.Tests.Mapeamentos {
    public class GestorTransacoesTestes : GestorTransacoes {
        private const string _cnnString = @"Data Source=np:\\.\pipe\LOCALDB#7D3EB568\tsql\query;AttachDbFileName=tests.mdf;Integrated Security=true";

        public override string ObtemCnnString() {
            return _cnnString;
        }
    }
}