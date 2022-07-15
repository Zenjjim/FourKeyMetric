using devops_metrics.ClientHandlers.Jira.ClientModels;
using Newtonsoft.Json;

namespace devops_metrics.ClientHandlers.Jira;

public class JiraHandler
{
    private readonly HttpClient _client;

    public JiraHandler(String token)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
    }

    public async Task<JiraTicketModel> GetTicket(String jira, String jiraTicket)
    {
        String path =
            $"https://{jira}.atlassian.net/rest/api/3/issue/{jiraTicket}";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var ticketDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<JiraTicketModel>(await ticketDataRaw);
    }

}
