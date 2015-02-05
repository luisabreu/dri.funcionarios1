using System;

namespace Domain.Messages.Relatorios {
    public class ResumoFuncionario {
        public int Id { get; set; }
        public string Nome { get; set; }
        public String Nif { get; set; }
        public String TipoFuncionario { get; set; }
    }
}