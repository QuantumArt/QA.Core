#pragma warning disable 1591
namespace QA.Core.Service
{
    public interface IManagedService
    {
        IServiceManager ServiceManager { get; set; }
    }
}
