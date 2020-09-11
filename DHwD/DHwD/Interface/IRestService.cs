using DHwD.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Interface
{
    public interface IRestService
    {
        Task<bool> RegisterNewUserAsync(UserRegistration item);
        Task<bool> CheckUserExistsAsync(UserRegistration item);
    }
}
