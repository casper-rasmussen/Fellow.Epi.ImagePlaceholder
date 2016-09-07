using EPiServer.Cms.Shell.UI.UIDescriptors;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using Fellow.Epi.ImagePlaceholder.Bootstrapper;
using Fellow.Epi.ImagePlaceholder.Infrastructure.Editor.Content.Definition;
using StructureMap;

namespace Fellow.Epi.ImagePlaceholder.Infrastructure.Initialization
{
	[InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DependencyResolverInitialization : IConfigurableModule
	{
		public void ConfigureContainer(ServiceConfigurationContext context)
		{
			context.Container.Configure(ConfigureContainer);

            context.ConfigurationComplete += (sender, args) =>
            {
				args.Container.Model.EjectAndRemoveTypes(type => type.Equals(typeof(MediaRepositoryDescriptor)));
                args.Container.Configure(ConfigureContainerAfter);
            };           
		}

		private static void ConfigureContainer(ConfigurationExpression container)
		{
			container.For<IContentRepositoryDescriptor>().Add<PlaceholderImageMediaRepositoryDescriptor>();
		}

		private static void ConfigureContainerAfter(ConfigurationExpression container)
		{
			container.AddRegistry<FellowEpiImagePlaceholderImplementationBootstrapper>();
     	}

		public void Initialize(InitializationEngine context)
		{
		}

		public void Uninitialize(InitializationEngine context)
		{
		}

		public void Preload(string[] parameters)
		{
		}
	}
}
