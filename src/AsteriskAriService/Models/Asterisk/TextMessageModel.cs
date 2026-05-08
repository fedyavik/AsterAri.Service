
namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// A text message
    /// </summary>
    public class TextMessageModel
    {
        /// <summary>
        /// A technology specific URI specifying the source of the message.
        /// For pjsip technology, any SIP URI can be specified.
        /// For xmpp, the URI must correspond to the client connection being used to send the message.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// A technology specific URI specifying the destination of the message.
        /// Valid technologies include pjsip, and xmp.
        /// The destination of a message should be an endpoint.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The text of the message
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Technology specific key/value pairs (JSON object) associated with the message
        /// </summary>
        public List<TextMessageVariableModel> Variables { get; set; }
    }
}