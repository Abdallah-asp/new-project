using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class User : IdentityUser
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Country_code { get; set; }
        public string Confirmation_code { get; set; }
        public ICollection<BookAuthor> Book_authors { get; set; }
        public ICollection<BorrowTheBook> Borrow_the_books { get; set; }
        public ICollection<FeedBack> Feed_backs { get; set; }
    }
}
