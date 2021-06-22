using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitis.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name_ar { get; set; }

        [Required, StringLength(120)]
        public string Name_en { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public DateTime Date_of_publication { get; set; }

        [Required]
        public int Pages_number { get; set; }

        [Required]
        public string Short_description { get; set; }

        public int Publishing_house_id { get; set; }
        [ForeignKey(nameof(Publishing_house_id))]
        public PublishingHouse Publishing_House { get; set; }

        public int Category_id { get; set; }
        [ForeignKey(nameof(Category_id))]
        public Category Category { get; set; }

        public ICollection<BookAuthor> Book_authors { get; set; }
        public ICollection<BorrowTheBook> Borrow_the_books { get; set; }
        public ICollection<FeedBack> Feed_backs { get; set; }
    }
}
