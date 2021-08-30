using System.Drawing;
using Microsoft.Extensions.Options;
using Models.Options;

namespace Uploader.Services
{
    public class ImageService
    {
        private MediaOptions MediaOptions { get; set; }

        public ImageService(IOptions<MediaOptions> mediaOptions)
        {
            MediaOptions = mediaOptions.Value;
        }

        public Image TextToImage(string text)
        {
            for (int i = 50; i < text.Length; i += 51)
            {
                text = text.Insert(i, "\n");
            }

            Font font = new("Arial", 20, FontStyle.Italic);
            float imageWidth = MediaOptions.Width;
            float imageHeight = MediaOptions.Height;
            Color textColor = Color.Black;
            Color backColor = Color.White;

            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)imageWidth, (int)imageHeight);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, (imageWidth - textSize.Width) / 2, (imageHeight - textSize.Width) / 2);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;
        }
    }
}
