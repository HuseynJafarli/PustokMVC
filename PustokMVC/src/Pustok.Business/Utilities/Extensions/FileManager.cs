using Microsoft.AspNetCore.Http;

namespace Pustok.Business.Utilities.Extensions
{
    public static class FileManager
    {
        public static string SaveFile(this IFormFile file, string rootPath, string folderName)
        {
            string fileName = file.FileName;

            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(rootPath, folderName, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
    }
}
