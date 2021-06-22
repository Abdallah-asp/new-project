using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class College
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name_ar { get; set; }

        [Required, StringLength(120)]
        public string Name_en { get; set; }

        public ICollection<BorrowTheBook> Borrow_The_Books { get; set; }

    }
}
