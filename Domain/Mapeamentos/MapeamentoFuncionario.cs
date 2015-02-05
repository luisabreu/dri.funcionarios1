using FluentNHibernate.Mapping;

namespace Domain.Mapeamentos {
    public class MapeamentoFuncionario : ClassMap<Funcionario> {
        public MapeamentoFuncionario() {
            Table("Funcionarios");
            Not.LazyLoad();
            Id(f => f.Id)
                .Access.CamelCaseField(Prefix.Underscore)
                .GeneratedBy.Identity();
            Version(f => f.Versao);
            Map(f => f.Nome)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable();
            Map(f => f.Nif)
                .Access.CamelCaseField(Prefix.Underscore)
                .Not.Nullable();

            HasMany(f => f.Contactos)
                .Access.CamelCaseField(Prefix.Underscore)
                .AsBag()
                .Table("Contactos")
                .KeyColumn("IdFuncionario")
                .Component(c => {
                               c.Map(ct => ct.TipoContacto)
                                   .CustomType<int>()
                                   .CustomSqlType("integer");
                               c.Map(ct => ct.Valor);
                           })
                .Cascade.All()
                .Not.LazyLoad();

            References(f => f.TipoFuncionario, "IdTipoFuncionario")
                .Cascade.None()
                .Not.LazyLoad();
        }
    }
}