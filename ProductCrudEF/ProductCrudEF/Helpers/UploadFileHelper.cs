namespace ProductCrudEF.Helpers
{
    public class UploadFileHelper
    {
        public async static Task<string> UploadFile(IFormFile file)
        {
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var filePath = Path.Combine(uploadDirectory, $"{file.FileName}{Path.GetExtension(file.FileName)}");

            using var fs = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fs);

            return filePath;
        }
    }
}