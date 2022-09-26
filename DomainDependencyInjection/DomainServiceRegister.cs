using Domain;
using Lamar;

namespace DomainDependencyInjection
{
    public class DomainServiceRegister
    {
        public static ServiceRegistry GetRegister()
        {
            ServiceRegistry registry = new();
            
            registry.For<Usuarios>().Use<Usuarios>();

            //registry.For<Empresas>().Use<Empresas>();

            return registry;
        }
    }
}