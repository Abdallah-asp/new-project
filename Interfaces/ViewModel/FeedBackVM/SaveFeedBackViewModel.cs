using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.FeedBackVM
{
    public class SaveFeedBackViewModel
    {
        public int Id { get; set; }

        public string Feed_back { get; set; }

        public int Rate { get; set; }

        public int Book_id { get; set; }

        public string Error { get; set; }
    }
}
