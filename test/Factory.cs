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
}