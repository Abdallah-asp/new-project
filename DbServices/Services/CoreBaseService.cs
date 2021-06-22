using DbServices.Base;
using DbServices.Model;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Services
{
    public class CoreBaseService : BaseService, ICoreBase
    {
        private DataContext _context;
        public CoreBaseService(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool SaveSingleImage(string root, IFormFile img, out string fileName)
        {
            string uniqueFileName = null;

            if (img != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
                string filePath = Path.Combine(root, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    img.CopyTo(fileStream);
                }
            }

            fileName = uniqueFileName;
            return true;
        }

        public string GenerateRandomCodeAsNumber()
        {
            Random rnd = new Random();
            int _min = 1000;
            int _max = 9999;
            int month = rnd.Next(_min, _max);

            return month.ToString();
        }

        public bool SaveSingleImageBase64(string root, string img, out string fileName)
        {
            var fileaName = "";
            var extention = "";

            var fileExtention = img.Substring(0, 5).ToUpper();

            if (fileExtention == "IVBOR")
                extention = ".png";

            if (fileExtention == "/9J/4")
                extention = ".jpg";

            if (fileExtention == "JVBER")
                extention = ".pdf";

            if (string.IsNullOrEmpty(extention) || string.IsNullOrWhiteSpace(extention))
            {
                fileName = null;
                return false;
            }

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[20];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            fileaName = finalString + extention;

            var bytes = Convert.FromBase64String(img);


            using (var fileStream = new FileStream(Path.Combine(root, fileaName), FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
            }

            fileName = fileaName;
            return true;
        }
    }
}
