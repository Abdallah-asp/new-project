using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserViewModel
{
    public class SaveUserViewModel
    {
        [Required(ErrorMessage = "This Field can't be empty")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This Field can't be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This Field can't be empty")]
        [StringLength(100, ErrorMessage = "Password is very weak !", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This Field can't be empty")]
        public string Role { get; set; }

        public IFormFile File { get; set; } = null;

        public string Image { get; set; }

        public List<string> Error { get; set; }
    }
}
