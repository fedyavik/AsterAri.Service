using AsteriskAriService.DtmfHandlers;

namespace AsteriskAriService.Factories.Dtmf
{
    public interface IDtmfFactory
    {
        public T CreateDtmfHandler<T>() where T : BaseDtmfHandler;
    }
}