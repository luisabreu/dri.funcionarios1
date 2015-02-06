﻿using System.Collections.Generic;
using Domain.Messages.Relatorios;

namespace site.Models {
    public class DadosPesquisa {
        public string NifOuNome { get; set; }
        public IEnumerable<ResumoFuncionario> Funcionarios { get; set; }
        public bool PesquisaEfetuada { get; set; }
    }
}