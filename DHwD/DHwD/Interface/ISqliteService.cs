using DHwD.Models;
using System.Threading.Tasks;

namespace DHwD.Interface
{
    interface ISqliteService
    {
        Task<UserRegistration> GetItemAsync();
        Task SaveUser(UserRegistration user);
        bool IsTableExists(string tableName);
        Task<JWTToken> GetToken();
        Task<bool> DeleteToken();
        Task SaveToken(JWTToken token);
    }
}
