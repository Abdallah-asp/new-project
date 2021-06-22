using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.CollegeVM
{
    public class SaveCollegeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This feild can't be empty"), StringLength(120)]
        public string Name_ar { get; set; }

        [Required(ErrorMessage = "This feild can't be empty"), StringLength(120)]
        public string Name_en { get; set; }

        public string Error { get; set; } = string.Empty;
    }
}
