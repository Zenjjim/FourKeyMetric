using devops_metrics.Entities;
using MongoDB.Driver;

namespace devops_metrics.Services;

public class IncidentService
{
    private readonly IMongoCollection<Incident> _incidents;
    public IncidentService()
    {
        _incidents = IncidentDb.Open();
    }
    
    public void InsertAllIncidentData(List<Incident> incidents)
    {

        try
        {
            _incidents.InsertMany(incidents, new InsertManyOptions
            {
                IsOrdered = false
            });
        }
        catch (MongoBulkWriteException){}
    } 
    
    public async Task<List<Incident>> GetIncidents(int intervalMonths, string? organization, string? project, string? repository)
    {;
        var builder = Builders<Incident>.Filter;
        var filter = builder.Gte("StartTime", DateTimeOffset.Now.AddMonths(intervalMonths).ToUnixTimeSeconds());
        if (organization != null)
        {
            var filterOrganization = builder.Eq("Organization", organization);
            filter &= filterOrganization;
            
        }
        if (project != null)
        {
            var filterProject = builder.Eq("Project", project);
            filter &= filterProject;
            
        }       
        if (repository != null)
        {
            var filterRepository = builder.Eq("Repository", repository);
            filter &= filterRepository;
        }
        
        
        return await _incidents.FindAsync(filter).Result.ToListAsync();
        
    }
}

