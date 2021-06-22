using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.PublishingHouseVM
{
    public class SavePublishingHouseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field can't be empty"), StringLength(120)]
        public string Name_ar { get; set; }

        [Required(ErrorMessage = "This field can't be empty"), StringLength(120)]
        public string Name_en { get; set; }

        [Required(ErrorMessage = "This field can't be empty")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field can't be empty"), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This field can't be empty")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field can't be empty")]
        public string The_state { get; set; }

        [Required(ErrorMessage = "This field can't be empty"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Error { get; set; }
    }
}
