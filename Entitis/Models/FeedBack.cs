using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class FeedBack
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Feed_back { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Rate { get; set; }

        public int Book_id { get; set; }
        [ForeignKey(nameof(Book_id))]
        public Book Book { get; set; }

        public string User_id { get; set; }
        [ForeignKey(nameof(User_id))]
        public User User { get; set; }
    }
}
