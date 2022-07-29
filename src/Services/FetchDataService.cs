using devops;
using devops_metrics.ClientHandlers.Azure;
using devops_metrics.ClientHandlers.Jira;
using devops_metrics.Entities;
using Newtonsoft.Json;

namespace devops_metrics.Services;
public class FetchDataService
{
    private readonly DeploymentService _deploymentService;
    private readonly ChangeService _changeService;
    private readonly IncidentService _incidentService;
    private readonly AzureHandler _azure;
    private readonly JiraHandler _jira;

    public FetchDataService()
    {
        _deploymentService = new DeploymentService();
        _changeService = new ChangeService();
        _incidentService = new IncidentService();
        
        _azure = new AzureHandler(Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($":{Environment.GetEnvironmentVariable("AZURE_TOKEN")}")));
        _jira = new JiraHandler(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Environment.GetEnvironmentVariable("JIRA_USER")}:{Environment.GetEnvironmentVariable("JIRA_TOKEN")}")));
    }

    public void FetchAllData()
    {
        List<Config> configs;
        using (StreamReader r = new StreamReader("config.json"))
        {
            string json = r.ReadToEnd();
            configs = JsonConvert.DeserializeObject<List<Config>>(json);
        }

        foreach (var config in configs)
        {
            GetAzureData(config.Platform, config.Organization, config.Jira);
        }
    }

    private void GetAzureData(String platform, String organization, String jira)
    {
        var deployments = new List<Deployment>();
        var changes = new List<Change>();
        var incidents = new List<Incident>();
        var prFixWords = Environment.GetEnvironmentVariable("FIX_WORDS").Split(',').ToList();
        var projects = _azure.GetProjects(organization).Result.Value;

        foreach (var project in projects)
        {
            var repositories = _azure.GetRepositories(organization, project).Result.Value;
            var definitions = _azure.GetDefinitions(organization, project).Result.Value;
            foreach (var definition in definitions)
            {
                var repository = repositories.Find(rep => rep.Name == definition.Name);
                if (repository == null) continue;
                var builds = _azure.GetBuilds(organization, project, definition, repository).Result.Value;
                foreach (var build in builds)
                {
                    deployments.Add(new Deployment(build.QueueTime.Value.ToUnixTimeSeconds(), build.FinishTime.Value.ToUnixTimeSeconds(), repository.Name, definition.Id.ToString(), project.Name, organization, build.RequestedFor.DisplayName, platform));
                }

                var pullRequests = _azure.GetPullRequests(organization, project, repository).Result.Value;
                foreach (var pullRequest in pullRequests)
                {
                    var pullRequestCommits =
                        _azure.GetPullRequestsCommits(organization, project, repository, pullRequest).Result.Value;
                        var nrOfCommits = pullRequestCommits.Count;
                        long pullRequestSize = 0;
                    try
                    {
                        foreach (var comment in pullRequestCommits)
                        {
                            var commentChanges = _azure.GetCommitChanges(organization, project, repository, comment)
                                .Result.ChangeCounts;
                            pullRequestSize += commentChanges.Add.HasValue ? commentChanges.Add.Value : 0;
                            pullRequestSize += commentChanges.Edit.HasValue ? commentChanges.Edit.Value : 0;
                            pullRequestSize += commentChanges.Delete.HasValue ? commentChanges.Delete.Value : 0;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("## SUM CHANGES:###");
                        Console.WriteLine(e);
                    }
                    
                    var isFixPullRequest = prFixWords.Any(s =>
                        pullRequest.Title.Contains(s, StringComparison.OrdinalIgnoreCase));

                    try
                    {
                        var finishTime = builds
                            .Where(build => build.QueueTime.Value >= pullRequest.ClosedDate.Value)
                            .OrderBy(build => build.QueueTime.Value).First().FinishTime.Value.ToUnixTimeSeconds();
                        
                        changes.Add(new Change(pullRequest.CreationDate.Value.ToUnixTimeSeconds(),
                            finishTime, pullRequestSize, nrOfCommits,
                            pullRequest.PullRequestId.ToString(), repository.DefaultBranch, repository.Name,
                            project.Name, organization, pullRequest.CreatedBy.DisplayName, platform));
                        
                        if (isFixPullRequest)
                        {
                            long startTime;
                            string? jiraTicketKey = null;
                            try
                            {
                                var jiraTicket = _jira.GetTicket(jira, pullRequest.Title.Split(" ").First()).Result;
                                startTime = jiraTicket.Fields.Created.ToUnixTimeSeconds();
                                jiraTicketKey = jiraTicket.Key;
                            }
                            catch (Exception)
                            {
                                startTime = pullRequest.CreationDate.Value.ToUnixTimeSeconds();
                            }
                            incidents.Add(new Incident(startTime, finishTime, jiraTicketKey, pullRequest.Title, repository.Name, project.Name, organization, platform )); 

                        }
                    }
                    catch (Exception){ }
                }
            }
            
            _deploymentService.InsertAllDeploymentData(deployments);
            _changeService.InsertAllChangeData(changes);
            _incidentService.InsertAllIncidentData(incidents);


        }
    }

}