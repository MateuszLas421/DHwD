using System.Threading.Tasks;

namespace DHwD.ViewModels.Base
{
    public interface IAsyncInit
    {
        Task Init { get; }
    }
}
