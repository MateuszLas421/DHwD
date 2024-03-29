﻿using Microsoft.AppCenter.Crashes;
using Models.ModelsMobile.Common;
using Models.Request;
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
        static HttpClient client = new HttpClient();

        public static async Task<TOut> PostExecuteNoAuthAsync<T, TOut>(string str, T item) where T : class    // TO Check
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            HttpResponseMessage response = new HttpResponseMessage();
            string json = JsonConvert.SerializeObject(item);
            string responseBody;
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                response = await client.PostAsync(str, content);   //  POST  // 
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                Crashes.TrackError(ex);
                return await Task.FromResult<TOut>(JsonConvert.DeserializeObject<TOut>(null));
            }

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"successfully saved.");
                return await Task.FromResult<TOut>(JsonConvert.DeserializeObject<TOut>(responseBody));
            }
            return await Task.FromResult<TOut>(JsonConvert.DeserializeObject<TOut>(null));

        }

        public static async Task<TOut> PostExecuteAsync<T, TOut>(string str, JWTToken jWT, T item) where T : class
        {
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })      //  POST  // 
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var serialized = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
#if DEBUG
                var test = JsonConvert.SerializeObject(item);
#endif
                using (HttpResponseMessage response = await client.PostAsync(str, serialized))
                {
                    try
                    {
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
        /// <typeparam name="T"></typeparam>
        /// <param name="getRequest"></param>
        /// <returns></returns>
        public static async Task<T> GetExecuteAsync<T>(JWTToken jWT, GetRequest getRequest) where T : class
        {
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                HttpResponseMessage response;
                string responseContent;
                try
                {
                    response = await client.GetAsync(getRequest.strURL);                       //  Get  
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

        public static void PrepareAuth(AuthenticationHeaderValue authenticationHeaderValue)
        {
            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        public static void PrepareNoAuth(AuthenticationHeaderValue authenticationHeaderValue)
        {
            client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }
    }
}
