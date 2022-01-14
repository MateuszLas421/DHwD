using System;
using DHwD.Service.Enums;

namespace DHwD.Service.Interfaces
{
    public interface IRequest
    {
        public IRequestType RequestType { get; set; }
        public EContentType ContentType { get; set; }
        public EAuth Auth { get; set; }
    }
}
