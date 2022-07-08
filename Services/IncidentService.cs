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
}

