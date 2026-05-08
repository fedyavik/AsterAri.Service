using AsteriskAriService.DtmfHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace AsteriskAriService.Factories.Dtmf
{
    public class DtmfFactory(IServiceProvider sp) : IDtmfFactory
    {
        public T CreateDtmfHandler<T>() where T : BaseDtmfHandler
        {
            return sp.GetRequiredService<T>();
        }
    }
}