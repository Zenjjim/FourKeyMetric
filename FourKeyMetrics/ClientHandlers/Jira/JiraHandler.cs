using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using FourKeyMetrics.ClientHandlers.Jira.ClientModels;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace  FourKeyMetrics.ClientHandlers.Azure;

public class JiraHandler
{
    private readonly HttpClient _client;

    public JiraHandler(String token)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
    }

    public async Task<JiraTicketModel> GetTicket(String jiraTicket)
    {
        String path =
            $"https://mollermobilitygroup.atlassian.net/rest/api/3/issue/{jiraTicket}";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var ticketDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<JiraTicketModel>(await ticketDataRaw);
    }

}
