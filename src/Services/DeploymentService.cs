

using devops_metrics.Entities;
using MongoDB.Driver;

namespace devops_metrics.Services;

public class DeploymentService
{
    private readonly IMongoCollection<Deployment> _deployments;
    public DeploymentService()
    {
        _deployments = DeploymentDb.Open();
    }


    public void InsertAllDeploymentData(List<Deployment> deployments)
    {

        try
        {
            _deployments.InsertMany(deployments, new InsertManyOptions
            {
                IsOrdered = false
            });
        }
        catch (MongoBulkWriteException){}
    }

    public async Task<List<Deployment>> GetDeployments(int intervalMonths, string? organization, string? project, string? repository)
    {;
        var filter = Builders<Deployment>.Filter.Gte("StartTime", DateTimeOffset.Now.AddMonths(intervalMonths).ToUnixTimeSeconds());
        if (organization != null)
        {
            var filterOrganization = Builders<Deployment>.Filter.Eq("Organization", organization);
            filter &= filterOrganization;
            
        }
        if (project != null)
        {
            var filterProject = Builders<Deployment>.Filter.Eq("Project", project);
            filter &= filterProject;
            
        }       
        if (repository != null)
        {
            var filterRepository = Builders<Deployment>.Filter.Eq("Repository", repository);
            filter &= filterRepository;
        }
        
        return await _deployments.FindAsync(filter).Result.ToListAsync();
        
    }
}

