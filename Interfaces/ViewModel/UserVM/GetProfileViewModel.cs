using Entitis.Models;
using Interfaces.ViewModel.BorrowTheBookVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserViewModel
{
    public class GetProfileViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Image_Url { get; set; }

        public List<GetBorrowTheBookViewModel> Borrows { get; set; }
    }
}
