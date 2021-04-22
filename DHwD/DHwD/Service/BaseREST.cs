using DHwD.Service.Respone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Service
{
    public static class BaseREST
    {
        static HttpClient client;

        public static async Task<BaseRespone> PostExecuteAsync<T>( string str, T content)  where T : HttpContent
        { 
            using (client = new HttpClient())
            {
                BaseRespone response = new BaseRespone();
                try { response = (BaseRespone)await client.PostAsync(str, content); }                                      //  POST  // 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return await Task.FromResult<BaseRespone>(response); }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return response;
                }
                return await Task.FromResult<BaseRespone>(response);
            }
        }
    }
}
