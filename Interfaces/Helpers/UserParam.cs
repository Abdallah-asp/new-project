using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Helpers
{
    public class UserParam
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 0;

        private int _pageSize = 10;

        public string Lang { get; set; }

        public int CategoryId { get; set; }

        public int BookId { get; set; }

        public string Role { get; set; }

        public int SubCategoryId { get; set; }

        public string Key { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}

