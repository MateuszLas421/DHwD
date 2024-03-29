﻿using System.Threading.Tasks;

namespace Models.Request
{
    public static class PrepareGetRequest
    {
        public static Task<GetRequest> AddOnlyValue(GetRequest getRequest, string parametr)
        {
            getRequest.strURL += parametr;
            return Task.FromResult<GetRequest>(getRequest);
        }

        public static Task<GetRequest> PrepareFirstParametr(GetRequest getRequest, string nameparametr, string parametr)
        {
            getRequest.strURL += nameparametr + "=" + parametr;
            return Task.FromResult<GetRequest>(getRequest);
        }
        public static Task<GetRequest> PrepareMoreParametr(GetRequest getRequest, string nameparametr, string parametr)
        {
            getRequest.strURL += "?" + nameparametr + "=" + parametr;
            return Task.FromResult<GetRequest>(getRequest);
        }
    }
}
