using Interfaces.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICoreBase : IService
    {
        void Add<T>(T entity) where T : class;
        Task<bool> SaveAll();
        bool SaveSingleImage(string root, IFormFile img, out string fileName);
        bool SaveSingleImageBase64(string root, string img, out string fileName);
        public string GenerateRandomCodeAsNumber();
    }
}
