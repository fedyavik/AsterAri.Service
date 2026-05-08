namespace AsteriskAriService.Models
{
    public abstract class StasisHandler
    {
        /// <summary>
        /// Handler for stasis
        /// </summary>
        /// <returns></returns>
        public abstract Task Handler();
    }
}