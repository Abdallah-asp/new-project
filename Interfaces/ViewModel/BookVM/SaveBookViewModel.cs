using Entitis.Models;
using Interfaces.ViewModel.CategoryVM;
using Interfaces.ViewModel.PublishingHouseVM;
using Interfaces.ViewModel.UserVM;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.BookVM
{
    public class SaveBookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This feild can't be empty"), StringLength(120)]
        public string Name_ar { get; set; }

        [Required(ErrorMessage = "This feild can't be empty"), StringLength(120)]
        public string Name_en { get; set; }

        [Required(ErrorMessage = "This feild can't be empty"), StringLength(120)]
        public string Short_description { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public DateTime Date_of_publication { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public int Pages_number { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public int? Publishing_house_id { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public int? Category_id { get; set; }

        public string Image { get; set; }

        public IFormFile File { get; set; }

        [Required(ErrorMessage = "This feild can't be empty")]
        public string Name { get; set; }

        public string Error { get; set; } = string.Empty;

        public List<GetCategoriesDrobDownListViewModel> Categories { get; set; }

        public List<GetPublishingHousesDrobDownListViewModel> PublishingHouses { get; set; }

        public List<UserDrobDownListViewModel>  users { get; set; }

    }
}
