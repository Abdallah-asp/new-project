using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserViewModel
{
    public class RegisterViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Name { get; set; }

        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        public string Image { get; set; }
    }
}
