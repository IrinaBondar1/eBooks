// Dependency Injection configuration
// NOTA: Pentru a activa DI complet, instalează pachetul: Install-Package Autofac.Integration.Mvc
// Momentan, controller-ele folosesc instanțiere manuală care funcționează corect

/*
using Autofac;
using Autofac.Integration.Mvc;

using NivelAccessDate;
using NivelServicii;
using NivelServicii.Cache;
using Repository_CodeFirst;

using System.Reflection;
using System.Web.Mvc;
*/

namespace eBooks_MVC
{
    public class ContainerConfigurer
    {
        public static void ConfigureContainer()
        {
            // Dependency Injection este dezactivat momentan
            // Controller-ele folosesc instanțiere manuală în constructori
            // Pentru a activa DI:
            // 1. Instalează: Install-Package Autofac.Integration.Mvc
            // 2. Decomentează codul de mai jos
            
            /*
            var builder = new ContainerBuilder();

            // Register MVC controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Register Accessors
            builder.RegisterType<AutorAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<CarteAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<CategorieAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<SerieAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<TipAbonamentAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<UtilizatorAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.RegisterType<IstoricCitireAccessor>()
                   .AsSelf()
                   .InstancePerRequest();

            // Register Services
            builder.RegisterType<AutorService>()
                   .As<IAutorService>()
                   .InstancePerRequest();

            builder.RegisterType<CarteService>()
                   .As<ICarteService>()
                   .InstancePerRequest();

            builder.RegisterType<AccesService>()
                   .As<IAccesService>()
                   .InstancePerRequest();

            // Register Cache (Singleton - shared across all requests)
            builder.RegisterType<MemoryCacheService>()
                   .As<ICache>()
                   .SingleInstance();

            // Register Context
            builder.RegisterType<eBooksContext>()
                   .AsSelf()
                   .InstancePerRequest();

            // Build container
            var container = builder.Build();

            // Set dependency resolver for MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            */
        }
    }
}
