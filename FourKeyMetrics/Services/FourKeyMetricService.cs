using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FourKeyMetrics.Service;

public class FourKeyMetricService
{
    private DeploymentService _deploymentService;
    private ChangeService _changeService;
    private AzureManager _azure;

    public FourKeyMetricService()
    {
        _deploymentService = new DeploymentService();
        _changeService = new ChangeService();
        _azure = new AzureManager(Environment.GetEnvironmentVariable("AZURE_TOKEN"));
    }

    public String Get()
    {
        return "Her kommer masse data";
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
            GetAzureData(config.Platform, config.Organization);
        }
    }

    public void GetAzureData(String platform, String organizaion)
    {
        var deployments = new List<Deployment>();
        var changes = new List<Change>();
        var prFixWords = new List<string>{"bug", "fix", "crash", "error", "issue"};

        var projects = _azure.GetProjects(organizaion).Result.Value;

        foreach (var project in projects)
        {
            var repositories = _azure.GetRepositories(organizaion, project).Result.Value;
            var definitions = _azure.GetDefinitions(organizaion, project).Result.Value;
            foreach (var definition in definitions)
            {
                var repository = repositories.Find(rep => rep.Name == definition.Name);
                if (repository == null) continue;
                var builds = _azure.GetBuilds(organizaion, project, definition, repository).Result.Value;
                foreach (var build in builds)
                {
                    deployments.Add(new Deployment(build.QueueTime.Value.ToUnixTimeSeconds(), build.FinishTime.Value.ToUnixTimeSeconds(), repository.Name, definition.Id.ToString(), project.Name, organizaion, build.RequestedFor.DisplayName, platform));
                }

                var pullRequests = _azure.GetPullRequests(organizaion, project, repository).Result.Value;
                foreach (var pullRequest in pullRequests)
                {
                    var pullRequestCommits =
                        _azure.GetPullRequestsCommits(organizaion, project, repository, pullRequest).Result.Value;
                        var nrOfCommits = pullRequestCommits.Count;
                        long pullRequestSize = 0;
                    try
                    {
                        foreach (var comment in pullRequestCommits)
                        {
                            var commentChanges = _azure.GetCommitChanges(organizaion, project, repository, comment)
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

                    long finishTime;
                    bool isInProduction;
                    try
                    {
                        var goodBuilds = builds
                            .Where(build => build.QueueTime.Value >= pullRequest.ClosedDate.Value)
                            .OrderBy(build => build.QueueTime.Value);
                        finishTime = goodBuilds.First().FinishTime.Value.ToUnixTimeSeconds();
                        isInProduction = true;
                    }
                    catch (Exception e)
                    {
                        finishTime = pullRequest.ClosedDate.Value.ToUnixTimeSeconds();
                        isInProduction = false;
                    }
                        var isFix = prFixWords.Any(s =>
                            pullRequest.Title.Contains(s, StringComparison.OrdinalIgnoreCase));
                        
                        changes.Add(new Change(pullRequest.CreationDate.Value.ToUnixTimeSeconds(),
                            finishTime, isInProduction, isFix, pullRequestSize, nrOfCommits,
                            pullRequest.PullRequestId.ToString(), repository.DefaultBranch, repository.Name,
                            project.Name, organizaion, pullRequest.CreatedBy.DisplayName, platform));
                }
                
            }
            
            _deploymentService.InsertAllDeploymentData(deployments);
            _changeService.InsertAllChangeData(changes);


        }
    }

}