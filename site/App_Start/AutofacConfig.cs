using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Domain;
using Domain.Messages;
using NHibernate;
using site.Helpers;

namespace site.App_Start {
    public class AutofacConfig {
        private static ISessionFactory _fabricaSessoes = new GestorTransacoes().ObtemFabricaSessoes();

        public static void RegisterForMvc() {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof (Funcionario).Assembly, typeof (TipoContacto).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();

            OverrideDependencyRegistration(builder);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
        
        public static void RegisterForWebApi(HttpConfiguration config) {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());


            builder.RegisterAssemblyTypes(typeof (Funcionario).Assembly, typeof (TipoContacto).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();

            OverrideDependencyRegistration(builder);

            var resolver = new AutofacWebApiDependencyResolver(builder.Build());
            config.DependencyResolver = resolver;
        }

        private static void OverrideDependencyRegistration(ContainerBuilder builder) {
            builder.Register(c => _fabricaSessoes.OpenSession())
                .As<ISession>()
                .InstancePerRequest();
        }
    }
}