using DHwD.Service.Respone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace DHwD.Service
{
    public static class BaseREST
    {
        static HttpClient client;

        //public BaseRespone PostExecuteAsync()
        //{
        //    using (client = new HttpClient())
        //    {
        //        BaseRespone response;
        //        try { response = await client.PostAsync(uri, content); }                                      //  POST  // 
        //        catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return BaseRespone; }

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Debug.WriteLine(@"successfully saved.");
        //            return BaseRespone;
        //        }
        //        return BaseRespone;
        //    }
        //}
    }
}
