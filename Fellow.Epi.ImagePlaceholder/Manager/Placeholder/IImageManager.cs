using System.IO;

namespace Fellow.Epi.ImagePlaceholder.Manager.Placeholder
{
	public interface IImageManager
	{
		string FileExtension { get; }

		Stream GetStream(int width, int height);
	}
}
