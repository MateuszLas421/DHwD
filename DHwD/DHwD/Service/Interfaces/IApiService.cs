using System;
namespace DHwD.Service.Interfaces
{
    public interface IApiService
    {
        IResponse InvokeRestRequest(IRequest request);
    }
}
