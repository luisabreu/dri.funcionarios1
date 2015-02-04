using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace Domain.Mapeamentos {
    public abstract class GestorTransacoes {

        public Configuration ObtemConfiguracao() {
            var configuration = Fluently.Configure()
                .Database(ConfiguraDb());
            MapeiaTiposDeAssembly(configuration);
            return configuration.BuildConfiguration();
        }

        public abstract string ObtemCnnString();

        public virtual IPersistenceConfigurer ConfiguraDb() {
            return MsSqlConfiguration.MsSql2008
                .ConnectionString(ObtemCnnString())
                .ShowSql();
        }

        protected virtual void MapeiaTiposDeAssembly(FluentConfiguration configuration) {
            configuration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Funcionario>());
        }
    }
}