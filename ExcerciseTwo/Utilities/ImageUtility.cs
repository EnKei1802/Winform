using System.IO;
using System.Text;

namespace ExcerciseTwo.Utilities
{
    public static class ImageUtility
    {
        private static readonly string ImageFolder = "Pictures";

        public static string UploadImage(string fullPath)
        {
            if(string.IsNullOrEmpty(fullPath)) return string.Empty;
            string filename = Path.GetFileName(fullPath);

            var folderPath = GetImageFolderPath();

            //Check whether Directory (Folder) exists.
            if (!Directory.Exists(folderPath +"\\" + ImageFolder))
            {
                //If Directory (Folder) does not exists Create it.
                Directory.CreateDirectory(folderPath + "\\" + ImageFolder);
            }
            var guid = Guid.NewGuid().ToString();
            var fileNameNew = new StringBuilder();
            fileNameNew.Append(guid).Append("-").Append(filename);
            var fileNameNewStr = fileNameNew.ToString();
            string pathImage = Path.Combine(folderPath,ImageFolder, fileNameNewStr);
            File.Copy(fullPath, pathImage, true);
            return fileNameNewStr;
        }

        public static string GetFullImagePath(string imagename)
        {
            var parentPath = GetImageFolderPath();
            return Path.Combine(parentPath,ImageFolder,imagename);
        }

        private static string GetImageFolderPath()
        {
            string startupPath = Directory.GetCurrentDirectory();
            string parentPath = Directory.GetParent(startupPath).Parent.Parent.FullName;
            return parentPath;
        }
        
        public static void RemoveImage(string pathImage)
        {
            File.Delete(GetFullImagePath(pathImage));
        }
    }
}
