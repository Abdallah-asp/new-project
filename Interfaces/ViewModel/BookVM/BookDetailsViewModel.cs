using Entitis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BookVM
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date_of_publication { get; set; }

        public int Pages_number { get; set; }

        public string Publishing_house_name { get; set; }

        public string Category_name { get; set; }

        public string Image { get; set; }

        public string Book_Author { get; set; }
    }
}
