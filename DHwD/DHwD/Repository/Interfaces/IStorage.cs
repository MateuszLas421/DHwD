using System;
using System.Threading.Tasks;

namespace DHwD.Repository.Interfaces
{
    public interface IStorage
    {
        Task SaveData(string key, string value);
        Task<string> ReadData(string key);
        Task ClearKey(string key);
        Task ClearAll();
    }
}
