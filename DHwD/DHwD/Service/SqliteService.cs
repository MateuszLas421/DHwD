using DHwD.Interface;
using DHwD.Model;
using SQLite;
using System.Threading.Tasks;

namespace DHwD.Service
{
    public class SqliteService : ISqliteService
    {
        SQLiteAsyncConnection db;
        public SqliteService()
        {
            db = new SQLiteAsyncConnection(SqlConst.DatabasePath);
        }
        public async Task<User> GetItemAsync()
        {
            var check = IsTableExists(nameof(User));
            if (check)
            {
                var userdb = await db.Table<User>().FirstOrDefaultAsync();
                return userdb;
            }
            return null;
        }
        public bool IsTableExists(string tableName)
        {
            try
            {
                var tableInfo = db.GetConnection().GetTableInfo(tableName);
                if (tableInfo.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task SaveUser(User user)
        {
            await db.CreateTableAsync<User>();
            await db.InsertAsync(user);
        }

    }
}
