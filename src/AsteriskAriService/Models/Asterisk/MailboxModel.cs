namespace AsteriskAriService.Models.Asterisk
{
    public class MailboxModel
    {
        /// <summary>
        /// Name of the mailbox.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Count of old messages in the mailbox.
        /// </summary>
        public int Old_messages { get; set; }

        /// <summary>
        /// Count of new messages in the mailbox.
        /// </summary>
        public int New_messages { get; set; }
    }
}