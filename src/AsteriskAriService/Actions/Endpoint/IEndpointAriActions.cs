using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Endpoint
{
    public interface IEndpointAriActions
    {
        /// <summary>
        /// List all endpoints.
        /// </summary>
        Task<List<EndpointModel>> ListAsync();

        /// <summary>
        /// Send a message to some technology URI or endpoint.
        /// </summary>
        /// <param name="to">The endpoint resource or technology specific URI to send the message to. Valid resources are sip, pjsip, and xmpp.</param>
        /// <param name="from">The endpoint resource or technology specific identity to send this message from. Valid resources are sip, pjsip, and xmpp.</param>
        /// <param name="body">The body of the message</param>
        /// <param name="variables"></param>
        /// <exception cref="AsterAriException">400 - Invalid parameters for sending a message.</exception>
        /// <exception cref="AsterAriException">404 - Endpoint not found</exception>
        Task SendMessageAsync(string to, string from, string? body = null, Dictionary<string, string>? variables = null);
        
        /// <summary>
        /// Refer an endpoint or technology URI to some technology URI or endpoint.
        /// </summary>
        /// <param name="to">The endpoint resource or technology specific URI that should be referred to somewhere. Valid resource is pjsip.</param>
        /// <param name="from">The endpoint resource or technology specific identity to refer from.</param>
        /// <param name="referTo">The endpoint resource or technology specific URI to refer to.</param>
        /// <param name="toSelf">- If true and "refer_to" refers to an Asterisk endpoint,
        /// the "refer_to" value is set to point to this Asterisk endpoint - so the referee is referred to Asterisk.
        /// Otherwise, use the contact URI associated with the endpoint.</param>
        /// <param name="variables"> The "variables" key in the body object holds technology specific
        /// key/value pairs to append to the message.
        /// These can be interpreted and used by the various resource types;
        /// for example, the pjsip resource type will add the key/value pairs as SIP headers.
        /// The "display_name" key is used by the PJSIP technology.
        /// Its value will be prepended as a display name to the Refer-To URI.</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters for referring.</exception>
        /// <exception cref="AsterAriException">404 - Endpoint not found</exception>
        Task ReferAsync(string to, string from, string referTo, bool toSelf, Dictionary<string, string>? variables = null);
        
        /// <summary>
        /// List available endpoints for a given endpoint technology.
        /// </summary>
        /// <param name="tech">Technology of the endpoints (sip,iax2,...)</param>
        /// <exception cref="AsterAriException">404 - Endpoints not found</exception>
        Task<List<EndpointModel>> ListByTechAsync(string tech);
        
        /// <summary>
        /// Details for an endpoint.
        /// </summary>
        /// <param name="tech">Technology of the endpoint</param>
        /// <param name="resource">ID of the endpoint</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters for sending a message.</exception>
        /// <exception cref="AsterAriException">404 - Endpoints not found</exception>
        Task<EndpointModel> GetAsync(string tech, string resource);

        /// <summary>
        /// Send a message to some endpoint in a technology.
        /// </summary>
        /// <param name="tech">Technology of the endpoint</param>
        /// <param name="resource">ID of the endpoint</param>
        /// <param name="from">The endpoint resource or technology specific identity to send this message from. Valid resources are sip, pjsip, and xmpp.</param>
        /// <param name="body">The body of the message</param>
        /// <param name="variables"></param>
        /// <exception cref="AsterAriException">400 - Invalid parameters for sending a message.</exception>
        /// <exception cref="AsterAriException">404 - Endpoints not found</exception>
        Task SendMessageToEndpointAsync(string tech, string resource, string from, string? body = null, Dictionary<string, string>? variables = null);
        
        /// <summary>
        /// Refer an endpoint or technology URI to some technology URI or endpoint.
        /// </summary>
        /// <param name="tech">Technology of the endpoint</param>
        /// <param name="resource">ID of the endpoint</param>
        /// <param name="from">The endpoint resource or technology specific identity to refer from.</param>
        /// <param name="referTo">The endpoint resource or technology specific URI to refer to.</param>
        /// <param name="toSelf">- If true and "refer_to" refers to an Asterisk endpoint,
        /// the "refer_to" value is set to point to this Asterisk endpoint - so the referee is referred to Asterisk.
        /// Otherwise, use the contact URI associated with the endpoint.</param>
        /// <param name="variables"> The "variables" key in the body object holds technology specific
        /// key/value pairs to append to the message.
        /// These can be interpreted and used by the various resource types;
        /// for example, the pjsip resource type will add the key/value pairs as SIP headers.
        /// The "display_name" key is used by the PJSIP technology.
        /// Its value will be prepended as a display name to the Refer-To URI.</param>
        /// <exception cref="AsterAriException">400 - Invalid parameters for referring.</exception>
        /// <exception cref="AsterAriException">404 - Endpoint not found</exception>
        Task ReferToEndpointAsync(string tech, string resource, string from, string referTo, bool toSelf, Dictionary<string, string>? variables = null);
    }
}