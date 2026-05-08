using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Asterisk.AsteriskInfo;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Asterisk
{
    public interface IAsteriskAriActions
    {
	    /// <summary>
		/// Retrieve a dynamic configuration object..
		/// </summary>
		/// <param name="configClass">The configuration class containing dynamic configuration objects.</param>
		/// <param name="objectType">The type of configuration object to retrieve.</param>
		/// <param name="id">The unique identifier of the object to retrieve.</param>
		/// <exception cref="AsterAriException">404 - {configClass|objectType|id} not found</exception>
		Task<List<ConfigTupleModel>> GetObjectAsync(string configClass, string objectType, string id);
	    
		/// <summary>
		/// Create or update a dynamic configuration object.
		/// </summary>
		/// <param name="configClass">The configuration class containing dynamic configuration objects.</param>
		/// <param name="objectType">The type of configuration object to create or update.</param>
		/// <param name="id">The unique identifier of the object to create or update.</param>
		/// <param name="fields">The body object should have a value that is a list of ConfigTuples,
		/// which provide the fields to update. Ex. [ { "attribute": "directmedia", "value": "false" } ]</param>
		/// <exception cref="AsterAriException">400 - Bad request body</exception>
		/// <exception cref="AsterAriException">403 - Could not create or update object</exception>
		/// <exception cref="AsterAriException">404 - {configClass|objectType} not found</exception>
		Task<List<ConfigTupleModel>> UpdateObjectAsync(string configClass, string objectType, string id, Dictionary<string, string>? fields = null);
		
		/// <summary>
		/// Delete a dynamic configuration object.
		/// </summary>
		/// <param name="configClass">The configuration class containing dynamic configuration objects.</param>
		/// <param name="objectType">The type of configuration object to delete.</param>
		/// <param name="id">The unique identifier of the object to delete.</param>
		/// <exception cref="AsterAriException">403 - Could not delete object</exception>
		/// <exception cref="AsterAriException">404 - {configClass|objectType|id} not found</exception>
		Task DeleteObjectAsync(string configClass, string objectType, string id);
		
		/// <summary>
		/// Gets Asterisk system information.
		/// </summary>
		/// <param name="only">Filter information returned.
		/// Allowed values: build, system, config, status.
		/// Allows comma separated values.</param>
		Task<AsteriskInfoModel> GetInfoAsync(string? only = null);
		
		/// <summary>
		/// Response pong message.
		/// </summary>
		Task<AsteriskPingModel> PingAsync();
		
		/// <summary>
		/// List Asterisk modules.
		/// </summary>
		Task<List<ModuleModel>> ListModulesAsync();
		
		/// <summary>
		/// Get Asterisk module information.
		/// </summary>
		/// <param name="moduleName">Module's name</param>
		/// <exception cref="AsterAriException">404 - Module could not be found in running modules.</exception>
		/// <exception cref="AsterAriException">409 - Module information could not be retrieved.</exception>
		Task<ModuleModel> GetModuleAsync(string moduleName);
		
		/// <summary>
		/// Load an Asterisk module.
		/// </summary>
		/// <param name="moduleName">Module's name</param>
		/// <exception cref="AsterAriException">409 - Module could not be loaded.</exception>
		Task LoadModuleAsync(string moduleName);
		
		/// <summary>
		/// Unload an Asterisk module.
		/// </summary>
		/// <param name="moduleName">Module's name</param>
		/// <exception cref="AsterAriException">404 - Module not found in running modules.</exception>
		/// <exception cref="AsterAriException">409 - Module could not be unloaded.</exception>
		Task UnloadModuleAsync(string moduleName);
		
		/// <summary>
		/// Reload an Asterisk module.
		/// </summary>
		/// <param name="moduleName">Module's name</param>
		/// <exception cref="AsterAriException">404 - Module not found in running modules.</exception>
		/// <exception cref="AsterAriException">409 - Module could not be reloaded.</exception>
		Task ReloadModuleAsync(string moduleName);
		
		/// <summary>
		/// Gets Asterisk log channel information.
		/// </summary>
		Task<List<LogChannelModel>> ListLogChannelsAsync();
		
		/// <summary>
		/// Adds a log channel.
		/// </summary>
		/// <param name="logChannelName">The log channel to add</param>
		/// <param name="configuration">levels of the log channel</param>
		/// <exception cref="AsterAriException">400 - Bad request body</exception>
		/// <exception cref="AsterAriException">409 - Log channel could not be created.</exception>
		Task AddLogAsync(string logChannelName, string configuration);
		
		/// <summary>
		/// Deletes a log channel.
		/// </summary>
		/// <param name="logChannelName">Log channels name</param>
		/// <exception cref="AsterAriException">404 - Log channel does not exist.</exception>
		Task DeleteLogAsync(string logChannelName);
		
		/// <summary>
		/// Rotates a log channel.
		/// </summary>
		/// <param name="logChannelName">Log channel's name</param>
		/// <exception cref="AsterAriException">404 - Log channel does not exist.</exception>
		Task RotateLogAsync(string logChannelName);
		
		/// <summary>
		/// Get the value of a global variable.
		/// </summary>
		/// <param name="variable">The variable to get</param>
		/// <exception cref="AsterAriException">400 - Missing variable parameter.</exception>
		Task<VariableModel> GetGlobalVarAsync(string variable);
		
		/// <summary>
		/// Set the value of a global variable.
		/// </summary>
		/// <param name="variable">The variable to set</param>
		/// <param name="value">The value to set the variable to</param>
		/// <exception cref="AsterAriException">400 - Missing variable parameter.</exception>
		Task SetGlobalVarAsync(string variable, string? value = null);
    }
}