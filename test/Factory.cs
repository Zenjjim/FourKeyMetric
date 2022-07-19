using devops_metrics.Entities;

namespace devops_metrics.test;

class Factory
{
    public static Deployment DeploymentFactory(
        DateTime startDate = default,
        DateTime finishDate = default,
        string? repository = null,
        string? definition = null,
        string? project = null,
        string? organization = null,
        string? developer = null,
        string? platform = null)
    { 
        
        return new Deployment(new DateTimeOffset(startDate).ToUnixTimeSeconds(), new DateTimeOffset(finishDate).ToUnixTimeSeconds(), repository, definition, project, organization, developer, platform);
    }
    public static Change ChangeFactory(
        DateTime startDate = default,
        DateTime finishDate = default,
        long? prSize = null,
        long? nrOfCommits = null,
        string? pullRequestId = null,
        string? branch = null,
        string? repository = null,
        string? project = null,
        string? organization = null,
        string? developer = null,
        string? platform = null)
    {
        return new Change(new DateTimeOffset(startDate).ToUnixTimeSeconds(), new DateTimeOffset(finishDate).ToUnixTimeSeconds(), (long)(prSize ?? Random.Shared.NextInt64()), (long)(nrOfCommits ?? Random.Shared.NextInt64()),  pullRequestId, branch, repository, project, organization, developer, platform);
    }    
    public static Incident IncidentFactory(
        DateTime startDate = default,
        DateTime finishDate = default,
        string? jira = null,
        string? title = null,
        string? repository = null,
        string? project = null,
        string? organization = null,
        string? platform = null)
    {
        return new Incident(new DateTimeOffset(startDate).ToUnixTimeSeconds(), new DateTimeOffset(finishDate).ToUnixTimeSeconds(), jira, title, repository, project, organization, platform);
    }
}