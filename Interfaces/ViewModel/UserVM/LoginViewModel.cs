using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserViewModel
{
    public class LoginViewModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
