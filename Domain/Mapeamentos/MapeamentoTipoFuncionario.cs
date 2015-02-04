﻿using Domain.Messages;
using FluentNHibernate.Mapping;

namespace Domain.Mapeamentos {
    public class MapeamentoTipoFuncionario : ClassMap<TipoFuncionario> {
        public MapeamentoTipoFuncionario() {
            Table("TipoFuncionario");
            Not.LazyLoad();
            ReadOnly();
            Id(funcionario => funcionario.Id)
                .Access.CamelCaseField()
                .Default(0)
                .GeneratedBy.Identity();
            Map(tf => tf.Descricao)
                .Not.Nullable();
        }
    }
}