using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserVM
{
    public class PhoneRequestViewModel
    {
        public string Phone { get; set; }
        public string Country_code { get; set; }
        public string New_password { get; set; }
        public string Confirm_password { get; set; }
        public string Code { get; set; }
    }
}
