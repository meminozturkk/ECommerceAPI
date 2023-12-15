using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;

namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService
    {
        IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        {

            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedFileName = NameOperation.CharRegulator(oldName);

            var files = Directory.GetFiles(path, regulatedFileName + "*");

            if (files.Length == 0) return regulatedFileName + "-1" + extension;

            int largestNumber = 0;
            foreach (var file in files)
            {
                int lastHyphenIndex = file.LastIndexOf("-");
                int fileNumber;
                if (int.TryParse(file[(lastHyphenIndex + 1)..^extension.Length], out fileNumber))
                {
                    largestNumber = Math.Max(largestNumber, fileNumber);
                }
            }

            return regulatedFileName + "-" + (largestNumber + 1) + extension;

        }


    }
}