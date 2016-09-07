using EPiServer.Core;

namespace Fellow.Epi.ImagePlaceholder.Models.Media
{
	public abstract class ImagePlaceholderMediaType : ImageData
	{
		public abstract int Width { get; set; }

		public abstract int Height { get; set; }
	}
}
