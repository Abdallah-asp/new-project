using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BorrowTheBookVM
{
    public class BorrowDetailsViewModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Borrowing_period_date { get; set; }

        public string Actual_return_date { get; set; }

        public string User_name { get; set; }

        public string Book_name { get; set; }

        public string College_name { get; set; }
    }
}
