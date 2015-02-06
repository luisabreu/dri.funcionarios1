using System.Collections.Generic;
using Domain.Messages;
using FuncionarioDto = Domain.Messages.Relatorios.Funcionario;

namespace site.Models {
    public class DadosFormularioFuncionario {
        public FuncionarioDto Funcionario { get; set; }
        public IEnumerable<TipoFuncionario> TiposFuncionario { get; set; }
        public bool Novo { get; set; }
    }
}