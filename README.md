# Fellow.Epi.ImagePlaceholder: Assisting and guiding your need for dummy images

This Episerver add-on provides editors with predefined and customizable image placeholders as part of their existing authoring workflow. It acts as a generator for any dummy images needed while authoring content or developing the platform. By default, it allows you, as a Episerver developer, to register predefined sizes - e.g. tailored to your 225x110px need for card images - or to simply let authors specify the desired proportions.

## Installation and Usage

You can get the latest version of Fellow.Epi.ImagePlaceholder through [Episervers NuGet Feed](http://nuget.episerver.com/en/OtherPages/Package/?packageId=Fellow.Epi.ImagePlaceholder)
Be aware that Fellow.Epi.ImagePlaceholder requires EPiServer.CMS.UI.Core version 9.6.0.0 or higher.

Please use this GitHub project for any issues, questions or other kinds of feedback.

Usage from a developers standpoint is straight forward. All it takes is registration of placeholder content types according to your need.

**Example 1: flexible placeholder image requesting editor to provide proportions**

```

	[ContentType(
		DisplayName = "Flexible Image",
		GUID = "[YOUR GUID HERE]")]
	public class FlexibleImageFileMediaType : ImagePlaceholderMediaType
	{
		[Display(Name = "Height")]
		[Range(10, 1000)]
		[Required]
		public override int Height { get; set; }

		[Display(Name = "Width")]
		[Range(10, 1000)]
		[Required]
		public override int Width { get; set; }
	}

```

**Example 2: fixed placeholder image**

```
	[ContentType(
		DisplayName = "Fixed Image",
		GUID = "[YOUR GUID HERE]")]
	public class FixedImageFileMediaType : ImagePlaceholderMediaType
	{
		[Ignore]
		public override int Height { get { return 400; } set { throw new NotSupportedException(); }}

		[Ignore]
		public override int Width { get { return 400; } set { throw new NotSupportedException(); } }
	}

```
	
## Configuration

It is possible to adjust the image generation behaviour and file extension by replacing or intercepting existing implemetation of IImageManager.

```
	public interface IImageManager
	{
		string FileExtension { get; }

		Stream GetStream(int width, int height);
	}
```

Default implementation of IImageManager relies on PNGs and uses the GDI+ drawing surface provided by Graphics in .NET.