using ModelLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAuthService
    {
        Task LoginUser(string email, string password);
        Task<bool> RegisterUser(UserDTO userDto);
    }
}
