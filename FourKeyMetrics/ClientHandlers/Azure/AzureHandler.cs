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
    public async Task<AzureRepositoryModel?> GetRepositories(String organization, AzureProjectModelValue project)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project.Name}/_apis/git/repositories?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var repositoriesDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureRepositoryModel>(await repositoriesDataRaw);
    }    
    public async Task<AzureDefinitionModel?> GetDefinitions(String organization, AzureProjectModelValue project)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project.Name}/_apis/build/definitions?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var definitionDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureDefinitionModel>(await definitionDataRaw);
    }
    public async Task<AzureBuildModel> GetBuilds(String organization, AzureProjectModelValue project, AzureDefinitionModelValue definition, AzureRepositoryModelValue repository)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project.Name}/_apis/build/builds?definitions={definition.Id.ToString()}&branchName={repository.DefaultBranch.Replace("/", "%2F")}&resultFilter=succeeded&statusFilter=completed&queryOrder=queueTimeDescending&api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var buildsDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureBuildModel>(await buildsDataRaw);
    }

    public async Task<AzurePullRequestModel> GetPullRequests(String organization, AzureProjectModelValue project, AzureRepositoryModelValue repository)
    {
        String path =
            $"https://dev.azure.com/{organization}/{project.Name}/_apis/git/repositories/{repository.Name}/pullRequests?searchCriteria.status=completed&searchCriteria.targetRefName={repository.DefaultBranch.Replace("/", "%2F")}&api-version=6.0&queryOrder=queueTimeDescending";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var pullRequestDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzurePullRequestModel>(await pullRequestDataRaw);
    }
    public async Task<AzurePullRequestCommitModel> GetPullRequestsCommits(String organization, AzureProjectModelValue project, AzureRepositoryModelValue repository, AzurePullRequestModelValue pullRequest)
    {
        String path = $"https://dev.azure.com/{organization}/{project.Name}/_apis/git/repositories/{repository.Name}/pullRequests/{pullRequest.PullRequestId}/commits?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var pullRequestCommitsRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzurePullRequestCommitModel>(await pullRequestCommitsRaw);
    }    
    public async Task<AzureCommitChangeModel> GetCommitChanges(String organization, AzureProjectModelValue project, AzureRepositoryModelValue repository, AzurePullRequestCommitModelValue comment)
    {
        String path = $"https://dev.azure.com/{organization}/{project.Name}/_apis/git/repositories/{repository.Name}/commits/{comment.CommitId}/changes?api-version=6.0";
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var pullRequestDataRaw = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AzureCommitChangeModel>(await pullRequestDataRaw);
    }
}
