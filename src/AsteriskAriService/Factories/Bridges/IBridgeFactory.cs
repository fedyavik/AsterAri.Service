using AsteriskAriService.Bridges;

namespace AsteriskAriService.Factories.Bridges
{
    public interface IBridgeFactory
    {
        /// <summary>
        /// Creating a bridge
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> CreateBridgeAsync<T>() where T : BaseBridgeAri;
    }
}