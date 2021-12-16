using System.Drawing;
namespace MemesProject.Helpers
{
    public static class ImageChanger
    {
        public static byte[] ImageToBytes(IFormFile image)
        {
            using (Image imageByte = Image.FromStream(image.OpenReadStream()))
            {
                using (var ms = new MemoryStream())
                {
                    imageByte.Save(ms, imageByte.RawFormat);
                    return ms.ToArray();
                }
            }
        }
    }
}
