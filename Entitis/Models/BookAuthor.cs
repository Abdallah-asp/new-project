using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class BookAuthor
    {
        public int Book_id { get; set; }
        public Book _Book { get; set; }

        public string User_id { get; set; }
        public User User { get; set; }
    }
}
