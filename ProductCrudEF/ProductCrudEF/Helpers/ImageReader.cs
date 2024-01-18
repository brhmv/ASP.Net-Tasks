namespace ProductCrudEF.Helpers
{
    public class ImageReader
    {
        public static byte[] ReadImage(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
