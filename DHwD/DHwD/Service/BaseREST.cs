using Microsoft.AppCenter.Crashes;
using Models.Request;
using Models.Respone;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Service
{
    public static class BaseREST
    {
        static HttpClient client;

        public static async Task<BaseRespone> PostExecuteAsync<T>( string str, T item)  where T : class
        { 
            using (client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                BaseRespone response = new BaseRespone();
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                try { response = (BaseRespone)await client.PostAsync(str, content); }                                      //  POST  // 
                catch (Exception ex) { 
                    Debug.WriteLine(ex.Message.ToString());
                    Crashes.TrackError(ex);
                    return await Task.FromResult<BaseRespone>(response); 
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return response;
                }
                return await Task.FromResult<BaseRespone>(response);
            }
        }

        public static async Task<TOut> PostExecuteAsync<T,TOut>(string str, T item) where T : class
        {
            using (client = new HttpClient())      //  POST  // 
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var serialized = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(str, serialized))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine(@"successfully saved.");
                            return await Task.FromResult(JsonConvert.DeserializeObject<TOut>(responseBody));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message.ToString());
                        Crashes.TrackError(ex);
                    }
                }
            }
            return await Task.FromResult(JsonConvert.DeserializeObject<TOut>(null));
        }

        /// <summary>
        /// Rest Get Method
        /// </summary>
        /// <param name="getRequest"></param>
        /// <returns>BaseRespone</returns>
        public static async Task<BaseRespone> GetExecuteAsync(GetRequest getRequest) 
        {
            using (client = new HttpClient())
            {
                BaseRespone response = new BaseRespone();
                try { response = (BaseRespone)await client.GetAsync(getRequest.strURL); }                 //  Get  
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                    Crashes.TrackError(ex);
                    return await Task.FromResult<BaseRespone>(response);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return response;
                }
                return await Task.FromResult<BaseRespone>(response);
            }
        }

        public static async Task<T> GetExecuteAsync<T>(GetRequest getRequest) where T : class
        {
            using (client = new HttpClient())
            {
                HttpResponseMessage response;
                string responseContent;
                try
                {
                    response =  await client.GetAsync(getRequest.strURL);                       //  Get  
                    responseContent = await response.Content.ReadAsStringAsync();
                }                
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                    Crashes.TrackError(ex);
                    return await Task.FromResult(JsonConvert.DeserializeObject<T>(null));
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(responseContent));
                }
                return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(responseContent));
            }
        }
        //private static async task<t> getrequest<t>(string uri)
        //{
        //    try
        //    {
        //        using (var client = new httpclient())
        //        {
        //            client.defaultrequestheaders.accept.add(
        //            new mediatypewithqualityheadervalue("application/json"));

        //            using (httpresponsemessage response = await client.getasync(uri))
        //            {
        //                response.ensuresuccessstatuscode();
        //                string responsebody = await response.content.readasstringasync();

        //                return jsonconvert.deserializeobject<t>(responsebody);
        //            }
        //        }
        //    }
        //    catch (exception ex)
        //    {
        //        console.writeline(ex.tostring());
        //    }
        //}
    }
}
