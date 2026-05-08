using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;

namespace AsteriskAriService.Actions.Mailbox
{
    public interface IMailboxesAriActions
    {
        /// <summary>
        /// List all mailboxes.
        /// </summary>
        Task<List<MailboxModel>> ListAsync();
        
        /// <summary>
        /// Retrieve the current state of a mailbox.
        /// </summary>
        /// <param name="mailboxName">Name of the mailbox</param>
        /// <exception cref="AsterAriException">404 - Mailbox not found.</exception>
        Task<MailboxModel> GetAsync(string mailboxName);
        
        /// <summary>
        /// Change the state of a mailbox. (Note - implicitly creates the mailbox).
        /// </summary>
        /// <param name="mailboxName">Name of the mailbox</param>
        /// <param name="oldMessages">Count of old messages in the mailbox</param>
        /// <param name="newMessages">Count of new messages in the mailbox</param>
        /// <exception cref="AsterAriException">404 - Mailbox not found.</exception>
        Task UpdateAsync(string mailboxName, int oldMessages, int newMessages);
        
        /// <summary>
        /// Destroy a mailbox.
        /// </summary>
        /// <param name="mailboxName">Name of the mailbox</param>
        /// <exception cref="AsterAriException">404 - Mailbox not found.</exception>
        Task DeleteAsync(string mailboxName);
    }
}