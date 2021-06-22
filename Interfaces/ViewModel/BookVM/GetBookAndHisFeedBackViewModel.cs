using Entitis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BookVM
{
    public class GetBookAndHisFeedBackViewModel
    {
        public Book Book { get; set; }
        public IQueryable<FeedBack> FeedBacks { get; set; }
    }
}
