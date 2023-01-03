using System;
using System.Threading.Tasks;
using DHwD.Repository.Interfaces;
using Models.ModelsMobile.Common;

namespace DHwD.Tools.Extensions
{
    public static class UserExtensions
    {
        public static async Task<UserRegistration> ReadUserAsync(this UserRegistration user, IStorage storage)
        {
            user.NickName = await storage.ReadData(Constans.Name);
            user.Token = await storage.ReadData(Constans.JWT);
            return user;
        }
    }
}
