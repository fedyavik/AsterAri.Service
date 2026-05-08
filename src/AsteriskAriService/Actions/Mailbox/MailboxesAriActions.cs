using System.Net.Http.Json;
using AsteriskAriService.Models.Asterisk;
using AsteriskAriService.Models.Exceptions;
using AsteriskAriService.Tools;

namespace AsteriskAriService.Actions.Mailbox
{
    public class MailboxesAriActions(HttpClient httpClient) : IMailboxesAriActions
    {
        public async Task<List<MailboxModel>> ListAsync()
        {
            var response = await httpClient.GetAsync("mailboxes");
            if (!response.IsSuccessStatusCode)
                throw new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode);
            return (await response.Content.ReadFromJsonAsync<List<MailboxModel>>())!;
        }

        public async Task<MailboxModel> GetAsync(string mailboxName)
        {
            var response = await httpClient.GetAsync($"mailboxes/{mailboxName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Mailbox not found.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
            return (await response.Content.ReadFromJsonAsync<MailboxModel>())!;
        }

        public async Task UpdateAsync(string mailboxName, int oldMessages, int newMessages)
        {
            var pathWithQuery = new QueryBuilder($"mailboxes/{mailboxName}")
                .Add("oldMessages", oldMessages)
                .Add("newMessages", newMessages);
            var response = await httpClient.PutAsync(pathWithQuery.ToString(),null);
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Mailbox not found.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }

        public async Task DeleteAsync(string mailboxName)
        {
            var response = await httpClient.DeleteAsync($"mailboxes/{mailboxName}");
            if (!response.IsSuccessStatusCode)
                throw (int)response.StatusCode switch
                {
                    404 => new AsterAriException("Mailbox not found.", (int)response.StatusCode),
                    _ => new AsterAriException($"Unknown response code {response.StatusCode} from ARI.", (int)response.StatusCode)
                };
        }
    }
}