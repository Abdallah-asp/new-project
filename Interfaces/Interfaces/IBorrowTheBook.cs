using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.BorrowTheBookVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IBorrowTheBook : IService
    {
        Task<string> SaveBorrowTheBook(SaveBorrowTheBookViewModel model);
        Task<PagedList<GetBorrowTheBookViewModel>> GetBorrowTheBooksWithPadination(UserParam param);
    }
}
