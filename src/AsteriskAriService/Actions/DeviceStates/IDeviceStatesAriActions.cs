using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.DeviceStates
{
    public interface IDeviceStatesAriActions
    {
        /// <summary>
        /// List all ARI controlled device states.
        /// </summary>
        Task<List<DeviceStateModel>> ListAsync();
        
        /// <summary>
        /// Retrieve the current state of a device.
        /// </summary>
        /// <param name="deviceName">Name of the device</param>
        Task<DeviceStateModel> GetAsync(string deviceName);
        
        /// <summary>
        /// Change the state of a device controlled by ARI. (Note - implicitly creates the device state).
        /// </summary>
        /// <param name="deviceName">Name of the device</param>
        /// <param name="deviceState">Device state value
        /// Allowed values: NOT_INUSE, INUSE, BUSY, INVALID, UNAVAILABLE, RINGING, RINGINUSE, ONHOLD</param>
        /// <exception cref="AsterAriException">404 - Device name is missing</exception>
        /// <exception cref="AsterAriException">409 - Uncontrolled device specified</exception>
        Task UpdateAsync(string deviceName, string deviceState);
        
        /// <summary>
        /// Destroy a device-state controlled by ARI.
        /// </summary>
        /// <param name="deviceName">Name of the device</param>
        /// <exception cref="AsterAriException">404 - Device name is missing</exception>
        /// <exception cref="AsterAriException">409 - Uncontrolled device specified</exception>
        Task DeleteAsync(string deviceName);
    }
}