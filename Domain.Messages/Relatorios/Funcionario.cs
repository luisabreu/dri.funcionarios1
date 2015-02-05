using System.Collections.Generic;

namespace Domain.Messages.Relatorios {
    public class Funcionario {
        public int Id { get; set; }
        public int Versao { get; set; }
        public string Nome { get; set; }
        public string Nif { get; set; }
        public TipoFuncionario TipoFuncionario{ get; set; }
        public IEnumerable<Contacto> Contactos { get; set; }
    }
}