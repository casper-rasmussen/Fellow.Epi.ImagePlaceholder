using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Fellow.Epi.ImagePlaceholder.Manager.Placeholder
{
	class ImageManager : IImageManager
	{
		public Stream GetStream(int width, int height)
		{
			MemoryStream memoryStream = new MemoryStream();
			
			if(width == 0 || height == 0)
				return memoryStream;
			
			//Create the empty image.
			Bitmap image = new Bitmap(width, height);

			//draw a useless line for some data
			Graphics imageData = Graphics.FromImage(image);

			imageData.SmoothingMode = SmoothingMode.AntiAlias;
			imageData.InterpolationMode = InterpolationMode.HighQualityBicubic;
			imageData.PixelOffsetMode = PixelOffsetMode.HighQuality;

			Rectangle textContainer = new Rectangle(0, 0, width, height);

			StringFormat sf = new StringFormat();
			sf.LineAlignment = StringAlignment.Center;
			sf.Alignment = StringAlignment.Center;

			imageData.FillRectangle(Brushes.LightGray, textContainer);
			imageData.DrawString(String.Format("{0} x {1}", width, height), new Font("Calibri", 10), Brushes.DarkGray, textContainer, sf);

			//Write to stream
			image.Save(memoryStream, ImageFormat.Png);

			memoryStream.Position = 0;
				
			return memoryStream;
			
		}

		public string FileExtension
		{
			get { return ".png"; }
		}
	}
}
