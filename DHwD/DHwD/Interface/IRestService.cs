﻿using DHwD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHwD.Interface
{
    public interface IRestService
    {
        Task<bool> RegisterNewUserAsync(UserRegistration item);
        Task<bool> CheckUserExistsAsync(UserRegistration item);
        Task<UserRegistration> GetUserAsync(UserRegistration item);
        Task<JWTToken> LoginAsync(UserRegistration item);
        Task<Team> GetTeamAsync();
        IAsyncEnumerable<Games> GetGames(JWTToken jWT);
    }
}