using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Application
{
    public interface IApplicationAriActions
    {
		/// <summary>
		/// List all applications.
		/// </summary>
		Task<List<ApplicationModel>> ListAsync();
		
		/// <summary>
		/// Get details of an application.
		/// </summary>
		/// <param name="applicationName">Application's name</param>
		/// <exception cref="AsterAriException">404 - Application does not exist.</exception>
		Task<ApplicationModel> GetAsync(string applicationName);
		
		/// <summary>
		/// Subscribe an application to an event source. Returns the state of the application after the subscriptions have changed
		/// </summary>
		/// <param name="applicationName">Application's name</param>
		/// <param name="eventSource">URI for event source (channel:{channelId}, bridge:{bridgeId}, endpoint:{tech}[/{resource}], deviceState:{deviceName})</param>
		/// <exception cref="AsterAriException">400 - Missing parameter.</exception>
		/// <exception cref="AsterAriException">404 - Application does not exist.</exception>
		/// <exception cref="AsterAriException">422 - Event source does not exist.</exception>
		Task<ApplicationModel> SubscribeAsync(string applicationName, params string[] eventSource);
		
		/// <summary>
		/// Unsubscribe an application from an event source. Returns the state of the application after the subscriptions have changed
		/// </summary>
		/// <param name="applicationName">Application's name</param>
		/// <param name="eventSource">URI for event source (channel:{channelId}, bridge:{bridgeId}, endpoint:{tech}[/{resource}], deviceState:{deviceName})</param>
		/// <exception cref="AsterAriException">400 - Missing parameter. event source scheme not recognized.</exception>
		/// <exception cref="AsterAriException">404 - Application does not exist.</exception>
		/// <exception cref="AsterAriException">404 - Application not subscribed to event source.</exception>
		/// <exception cref="AsterAriException">422 - Event source does not exist.</exception>
		Task<ApplicationModel> UnsubscribeAsync(string applicationName, params string[] eventSource);
		
		/// <summary>
		/// Filter application events types.
		/// Allowed and/or disallowed event type filtering can be done.
		/// The body (parameter) should specify a JSON key/value object that describes the type of event filtering needed.
		/// One, or both of the following keys can be designated:
		/// <br /><br />"allowed" - Specifies an allowed list of event types
		/// <br />"disallowed" - Specifies a disallowed list of event types<br />
		/// <br />Further, each of those key's value should be a JSON array that holds zero, or more JSON key/value objects.
		/// Each of these objects must contain the following key with an associated value:
		/// <br /><br />"type" - The type name of the event to filter<br /><br />
		/// The value must be the string name (case-sensitive) of the event type that needs filtering.
		/// For example:<br /><br />{ "allowed": [ { "type": "StasisStart" }, { "type": "StasisEnd" } ] }<br /><br />
		/// As this specifies only an allowed list, then only those two event type messages are sent to the application.
		/// No other event messages are sent.<br /><br />The following rules apply:<br />
		/// <br />* If the body is empty, both the allowed and disallowed filters are set empty.
		/// <br />* If both list types are given then both are set to their respective values
		/// (note, specifying an empty array for a given type sets that type to empty).
		/// <br />* If only one list type is given then only that type is set. The other type is not updated.
		/// <br />* An empty "allowed" list means all events are allowed.
		/// <br />* An empty "disallowed" list means no events are disallowed.
		/// <br />* Disallowed events take precedence over allowed events if the event type is specified in both lists.
		/// </summary>
		/// <param name="applicationName">Application's name</param>
		/// <param name="filter">Specify which event types to allow/disallow</param>
		/// <exception cref="AsterAriException">400 - Bad request.</exception>
		/// <exception cref="AsterAriException">404 - Application does not exist.</exception>
		Task<ApplicationModel> FilterAsync(string applicationName, object? filter = null);
    }
}