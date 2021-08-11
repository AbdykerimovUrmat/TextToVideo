using System.Drawing;
using System.Speech.Synthesis;
using AForge.Video.FFMPEG;

namespace Uploader.Services
{
    public class ImageService
    {
        public Image DrawText(string text, Font font, float imageWidth, float imageHeight, Color textColor, Color backColor)
        {
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

        public void CreateVideo(Image image, string path, string videoName, int frameRate, int lengthInSeconds)
        {
            using var videoWriter = new VideoFileWriter();
            videoWriter.Open(path + videoName, image.Width, image.Height, frameRate, VideoCodec.MPEG4, 1000000);

            for(int s = 0; s < lengthInSeconds; s++)
            {
                for(int f = 0; f < frameRate; f++)
                {
                    videoWriter.WriteVideoFrame(image as Bitmap);
                }
            }

            videoWriter.Close();
        }
    }
}
