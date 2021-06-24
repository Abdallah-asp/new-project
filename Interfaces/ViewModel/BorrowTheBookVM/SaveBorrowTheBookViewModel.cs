using Entitis.Models;
using Interfaces.ViewModel.BookVM;
using Interfaces.ViewModel.CollegeVM;
using Interfaces.ViewModel.UserVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BorrowTheBookVM
{
    public class SaveBorrowTheBookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public DateTime Borrowing_period_date { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public DateTime Actual_return_date { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public string User_id { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public int? Book_id { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public int? College_id { get; set; }

        public List<GetCollegeDrobDownListViewModel> Colleges { get; set; }

        public List<GetBooksDropDownListViewModel> Books { get; set; }

        public List<UserDrobDownListViewModel> Users { get; set; }

        public string Error { get; set; }
    }
}
