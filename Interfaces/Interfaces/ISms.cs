using Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ISms : IService
    {
        Task<bool> SendMessage(string phone, string body);
    }
}
