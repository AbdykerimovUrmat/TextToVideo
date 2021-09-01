using System.Drawing;

namespace Uploader.Helpers
{
    public static class ImageExtensions
    {
        public static byte[] ToBytes(this Image image)
        {
            ImageConverter imageConverter = new();
            byte[] bytes = (byte[]) imageConverter.ConvertTo(image, typeof(byte[]));
            return bytes;
        }
    }
}
