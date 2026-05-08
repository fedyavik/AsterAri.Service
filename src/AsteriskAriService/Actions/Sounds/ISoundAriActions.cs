using AsteriskAriService.Models.Asterisk;

namespace AsteriskAriService.Actions.Sounds
{
    public interface ISoundAriActions
    {
        /// <summary>
        /// List all sounds.
        /// </summary>
        /// <param name="lang">Lookup sound for a specific language.</param>
        /// <param name="format">Lookup sound in a specific format.</param>
        Task<List<SoundModel>> ListAsync(string? lang = null, string? format = null);
        
        /// <summary>
        /// Get a sound's details.
        /// </summary>
        /// <param name="soundId">Sound's id</param>
        Task<SoundModel> GetAsync(string soundId);
    }
}