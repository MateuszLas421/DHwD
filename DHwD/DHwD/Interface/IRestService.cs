using DHwD.Models;
using Models.ModelsDB;
using Models.ModelsMobile;
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

        IAsyncEnumerable<MobileTeam> GetTeams(JWTToken jWT, int IdGame);

        Task<bool> CreateNewTeam(JWTToken jWT, Team item);

        Task<bool> JoinToTeam(JWTToken jWT, Team item);

        Task<TeamMembers> GetMyTeams(JWTToken jWT, int idgame);

        IAsyncEnumerable<TeamMembers> GetTeamMembers(JWTToken jWT, int IdGame);

        Task<bool> CheckTeamPass(JWTToken jWT, int idTeam, string hashpass);

        Task<List<Location>> GetLocationAsync(JWTToken jWT, Team team);
    }
}
