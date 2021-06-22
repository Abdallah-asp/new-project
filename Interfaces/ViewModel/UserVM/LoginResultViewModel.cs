using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModel.UserViewModel
{
    public class LoginResultViewModel
    {
        public JwtSecurityToken Token { get; set; }
    }
}
