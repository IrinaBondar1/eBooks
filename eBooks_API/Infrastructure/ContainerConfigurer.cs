using Autofac;
using Autofac.Integration.WebApi;

using NivelAccessDate;

using NivelServicii;
using NivelServicii.Cache;

using Repository_CodeFirst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;


namespace eBooks_API.Infrastructure
{
    public class ContainerConfigurer
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<eBooksContext>()
                   .As<IeBooksContext>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<AutorAccessor>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<CarteAccessor>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<AutorService>()
                   .As<IAutorService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<CarteService>()
                   .As<ICarteService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<MemoryCacheService>()
                   .As<ICache>()
                   .SingleInstance();
            /*
            // Singleton 

            builder.RegisterType<TestService>()
                    .As<ITestService>()
                    .SingleInstance(); 

            // Scope

            builder.RegisterType<TestService>()
                    .As<ITestService>()
                    .InstancePerLifetimeScope();

            //Transient

            builder.RegisterType<TestService>()
                    .As<ITestService>()
                    .InstancePerDependency();

            */

            var config = GlobalConfiguration.Configuration;
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}