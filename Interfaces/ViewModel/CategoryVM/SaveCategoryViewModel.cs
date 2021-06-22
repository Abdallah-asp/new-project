using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Interfaces.ViewModel.CategoryVM
{
    public class SaveCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field can't be empty")]
        public string Name_ar { get; set; }

        [Required(ErrorMessage = "This field can't be empty")]
        public string Name_en { get; set; }

        public string Error { get; set; }
    }
}
