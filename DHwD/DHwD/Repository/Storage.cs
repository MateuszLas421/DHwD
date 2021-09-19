using System;
using System.Threading.Tasks;
using DHwD.Repository.Interfaces;
using Xamarin.Essentials;

namespace DHwD.Repository
{
    public class Storage : IStorage
    {
        public Storage(ILogs logs)
        {
            _logsRepository = logs;
        }

        #region variables
        private ILogs _logsRepository;
        #endregion

        public async Task SaveData(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                _logsRepository.LogError(ex);
            }
        }

        public async Task<string> ReadData(string key)
        {
            try
            {
                return await Task.FromResult(await SecureStorage.GetAsync(key));
            }
            catch (Exception ex)
            {
                _logsRepository.LogError(ex);
                return String.Empty;
            }
        }

        public async Task ClearKey(string key)
        {
            try
            {
                await Task.Run(() => SecureStorage.Remove(key));
            }
            catch (Exception ex)
            {
                _logsRepository.LogError(ex);
            }
        }

        public async Task ClearAll()
        {
            try
            {
                await Task.Run(() => SecureStorage.RemoveAll());
            }
            catch (Exception ex)
            {
                _logsRepository.LogError(ex);
            }
        }
    }
}
