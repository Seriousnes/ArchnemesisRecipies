using AutoMapper;
using System.Reflection;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace ArchnemesisRecipies.Utility
{
    public static class WrappedMapper
    {
        private static IConfigurationProvider _configuration;
        private static IMapper _instance;

        private static IConfigurationProvider Configuration
        {
            get => _configuration ?? throw new InvalidOperationException("Not initialized.");
            set => _configuration = (_configuration == null) ? value : throw new InvalidOperationException("Already initialized.");
        }

        public static IMapper Mapper
        {
            get => _instance ?? throw new InvalidOperationException("Not initialized");
            private set => _instance = value;
        }

        public static void Initialize(Action<IMapperConfigurationExpression> config)
        {
            Initialize(new MapperConfiguration(config));
        }

        public static void Initialize(MapperConfiguration config)
        {
            Configuration = config;
            Mapper = Configuration.CreateMapper();
        }

        public static void AssertConfigurationIsValid() => Configuration.AssertConfigurationIsValid();
    }

    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            WrappedMapper.Initialize(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
    }
}
