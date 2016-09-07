using System;
using System.Collections.Generic;
using EPiServer.Cms.Shell.UI.UIDescriptors;
using Fellow.Epi.ImagePlaceholder.Models.Media;

namespace Fellow.Epi.ImagePlaceholder.Infrastructure.Editor.Content.Definition
{
	public class PlaceholderImageMediaRepositoryDescriptor : MediaRepositoryDescriptor
	{
		public override IEnumerable<Type> CreatableTypes
		{
			get
			{
				return new[] { typeof(ImagePlaceholderMediaType) };
			}
		}

	}
}
