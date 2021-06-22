using Entitis.Models;
using Interfaces.Base;
using Interfaces.Helpers;
using Interfaces.ViewModel.UserViewModel;
using Interfaces.ViewModel.UserVM;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IUser : IService
    {
        JwtSecurityToken GenerateToken(IList<string> usersRole, User user);
        Task<LoginResultViewModel> Login(LoginViewModel model);
        Task<bool> Register(RegisterViewModel model);
        string GetRoleIdByName(string name);
        string GetUserIdByName(string Name);
        string GetUserNameById(string id);
        Task<User> SaveUser(SaveUserViewModel model, string root);
        Task<User> GetUserByPhoneNumber(string phone);
        Task<PagedList<GetUserViewModel>> GetUsersWithPagination(UserParam param);
        Task<bool> IsEmailExistBefore(string email);
        Task<bool> IsUserExistInDb(string name);
        Task<List<UserDrobDownListViewModel>> GetUserDrobDownList(string role = null);
        Task<GetProfileViewModel> GetProfile(string id, UserParam param, string root);
        Task<EditProfileViewModel> EditProfile(string id, EditProfileViewModel model, string root);
    }
}
