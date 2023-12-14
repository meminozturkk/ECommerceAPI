using ECommerceAPI.Application.Services;

using ECommerceAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
           
        }

        private async Task<string> FileRenameAsync(string path,string fileName,bool first = true)
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

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();
            foreach (IFormFile file in files)
            {
               string fileNewName = await  FileRenameAsync(uploadPath, file.FileName);

               bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);
                
               
            }
            if(results.TrueForAll(r=>r.Equals(true))) {
                return datas;
            }
            //todo if gecerli degikse uyarici exception olustur
            return null;
        }
    }
}
