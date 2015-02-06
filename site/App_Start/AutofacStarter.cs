using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Domain;
using Domain.Messages;
using NHibernate;
using site.Helpers;

namespace site.App_Start {
    public class AutofacStarter {
        public static void RegisterForMvc() {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof (Funcionario).Assembly, typeof (TipoContacto).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();

            OverrideDependencyRegistration(builder);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

        private static void OverrideDependencyRegistration(ContainerBuilder builder) {
            builder.Register(c => new GestorTransacoes().ObtemFabricaSessoes())
                .As<ISession>()
                .InstancePerRequest();
        }
    }
}