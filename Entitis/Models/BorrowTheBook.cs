using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class BorrowTheBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime Borrowing_period_date { get; set; }

        [Required]
        public DateTime Actual_return_date { get; set; }

        public string User_id { get; set; }
        [ForeignKey(nameof(User_id))]
        public User User { get; set; }

        public int Book_id { get; set; }
        [ForeignKey(nameof(Book_id))]
        public Book Book { get; set; }

        public int College_id { get; set; }
        [ForeignKey(nameof(College_id))]
        public College College { get; set; }
    }
}
