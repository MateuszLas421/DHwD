using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.ViewModels.Base
{
    public interface IAsyncInit
    {
        Task Init { get; }
    }
}
