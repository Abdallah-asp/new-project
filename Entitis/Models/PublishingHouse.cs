using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class PublishingHouse
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name_ar { get; set; }

        [Required, StringLength(120)]
        public string Name_en { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string The_state { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
