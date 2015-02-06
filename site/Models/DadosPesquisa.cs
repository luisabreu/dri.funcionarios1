using System.Collections.Generic;
using Domain.Messages.Relatorios;

namespace site.Models {
    public class DadosPesquisa {
        public string Pesquisa { get; set; }
        public IEnumerable<ResumoFuncionario> Funcionarios { get; set; }
    }
}