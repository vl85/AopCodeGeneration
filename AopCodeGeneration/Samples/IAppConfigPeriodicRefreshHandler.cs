using System;

namespace AopCodeGeneration.Samples
{
    public interface IAppConfigPeriodicRefreshHandler : IDisposable
    {
        void Start();
    }
}