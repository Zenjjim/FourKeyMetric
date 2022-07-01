using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace  FourKeyMetrics.ClientHandlers.Azure;

public class AzureManager
{
    private readonly HttpClient _client;

    public AzureManager(String token)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
    }

    public async Task<AzureProjectModel?> GetProjects(String organization)
    {
        String path =
            $"https://dev.azure.com/{organization}/_apis/projects?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var projectsDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureProjectModel>(await projectsDataRaw);
    }
    
    public async Task<AzureRepositoryModel?> GetRepositories(String organization, String project)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var repositoriesDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureRepositoryModel>(await repositoriesDataRaw);
    }    
    public async Task<AzureDefinitionModel?> GetDefinitions(String organization, String project)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project}/_apis/build/definitions?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var definitionDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureDefinitionModel>(await definitionDataRaw);
    }
    
    public async Task<AzureBuildModel> GetBuilds(String organization, String project, String definition, String branch)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project}/_apis/build/builds?definitions={definition}&branchName={branch.Replace("/", "%2F")}&resultFilter=succeeded&statusFilter=completed&queryOrder=queueTimeDescending&api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var buildsDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureBuildModel>(await buildsDataRaw);
    }
    
}
