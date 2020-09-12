using DHwD.Model;
using System.Threading.Tasks;

namespace DHwD.Interface
{
    interface ISqliteService
    {
        Task<User> GetItemAsync();
        Task SaveUser(User user);
        bool IsTableExists(string tableName);
    }
}
