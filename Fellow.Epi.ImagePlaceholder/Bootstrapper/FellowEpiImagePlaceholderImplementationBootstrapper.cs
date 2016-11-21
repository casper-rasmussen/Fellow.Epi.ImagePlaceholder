using EPiServer;
using EPiServer.Core.Internal;
using Fellow.Epi.ImagePlaceholder.Infrastructure.Editor.Content;
using Fellow.Epi.ImagePlaceholder.Manager.Placeholder;
using StructureMap.Configuration.DSL;

namespace Fellow.Epi.ImagePlaceholder.Bootstrapper
{
	public class FellowEpiImagePlaceholderImplementationBootstrapper : Registry
	{
		public FellowEpiImagePlaceholderImplementationBootstrapper()
		{
			this.For<IImageManager>().Use<ImageManager>();

			//Episerver overrides
			this.For<DefaultContentProvider>().Use<PlaceholderImageContentProvider>();
		}
	}
}
