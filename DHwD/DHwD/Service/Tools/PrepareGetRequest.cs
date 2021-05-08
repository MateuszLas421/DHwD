using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Request
{
    public static class PrepareGetRequest
    {

        public static Task<GetRequest> PrepareFirstParametr(GetRequest getRequest, string nameparametr, string parametr)
        {
            getRequest.strURL += nameparametr + "=" + parametr;
            return Task.FromResult<GetRequest>(getRequest);
        }
    }
}
