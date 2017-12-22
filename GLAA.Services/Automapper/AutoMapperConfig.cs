using System.Reflection;
using AutoMapper;

namespace GLAA.Services.Automapper
{
    public class AutoMapperConfig
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(Assembly.GetExecutingAssembly());
            });            

            config.AssertConfigurationIsValid();

            return config;         
        }
    }
}