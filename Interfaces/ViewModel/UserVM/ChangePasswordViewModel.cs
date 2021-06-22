using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserVM
{
    public class ChangePasswordViewModel
    {
        public string Old_password { get; set; }

        public string New_password { get; set; }

        public string Confirm_password { get; set; }

        public string Error { get; set; }
    }
}
