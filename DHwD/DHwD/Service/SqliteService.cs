using DHwD.Interface;
using DHwD.Models;
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
        #region USER
        public async Task<UserRegistration> GetItemAsync()
        {
            var check = IsTableExists(nameof(UserRegistration));
            if (check)
            {
                var userdb = await db.Table<UserRegistration>().FirstOrDefaultAsync();
                return userdb;
            }
            return null;
        }

        public async Task SaveUser(UserRegistration user)
        {
            var exist = IsTableExists(nameof(UserRegistration));
            if (!exist)
            {
                await db.CreateTableAsync<UserRegistration>();
            }
            await db.InsertAsync(user);
        }

        #endregion

        #region Token
        public async Task<bool> DeleteToken()
        {
            var exist = IsTableExists(nameof(JWTToken));
            if (exist)
            {
                //await db.DeleteAllAsync<JWTToken>();
                return true;
            }
            return false;
        }

        public async Task<JWTToken> GetToken()
        {
            bool check = IsTableExists(nameof(JWTToken));
            if (check)
            {
                var token = await db.Table<JWTToken>().FirstOrDefaultAsync();
                return token;
            }
            return null;
        }
        public async Task SaveToken(JWTToken jWT)
        {
            bool check = IsTableExists(nameof(JWTToken));
            if (!check)
            {
                await db.CreateTableAsync<JWTToken>();
            }
            await DeleteToken();
            await db.InsertAsync(jWT);
        }
        #endregion

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



    }
}
