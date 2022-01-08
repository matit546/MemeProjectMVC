using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MemesProject.Helpers
{
    public static class ImageChanger
    {
        public static byte[] ImageToBytes(IFormFile image)
        {
            using (Image imageByte = Resize(Image.FromStream(image.OpenReadStream())))
            {
                using (var ms = new MemoryStream())
                {
                    imageByte.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
        }
       
        public static Image Resize(Image image)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            double ratio = imageWidth / 400;
            int finalHeight = (int)(imageHeight / ratio);

            var destRect = new Rectangle(0, 0, 400, finalHeight);
            var destImage = new Bitmap(400, finalHeight);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return (Image)destImage;
        }
    }
}
