using Ecom.core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Service
{
    class ImageManagemntServce : IImageManagemntServce
    {
        private readonly IFileProvider fileProvider;
        public ImageManagemntServce(IFileProvider _fileProvider)
        {
         this.fileProvider = _fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var SaveimageSrc = new List<string>();
            var ImageDirectory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(ImageDirectory) is not true)
            {
                Directory.CreateDirectory(ImageDirectory);
            }
            foreach (var item in files)
            {
                if (item.Length>0)
                {
                    var imageName = item.FileName;
                    var imageSrc = $"Images/{src}/{imageName}";
                    var root = Path.Combine(ImageDirectory, imageName);
                    using (FileStream stream = new FileStream(root , FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveimageSrc.Add(imageSrc);
                }
            }
            return SaveimageSrc;
        }

        public void DeleteImageAsync(string src)
        {
           var info =fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}
