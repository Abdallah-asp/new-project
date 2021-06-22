using Entitis.Models;
using Interfaces.ViewModel.CategoryVM;
using Interfaces.ViewModel.PublishingHouseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BookVM
{
    public class GetBookViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category_name { get; set; }

        public string Image { get; set; }

    }
}
