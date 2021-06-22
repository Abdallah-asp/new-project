using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BorrowTheBookVM
{
    public class GetBorrowTheBookViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime Borrowing_period_date { get; set; }

        public DateTime Actual_return_date { get; set; }

        public string User_name { get; set; }

        public string Book_name { get; set; }

        public string College_name { get; set; }
    }
}
