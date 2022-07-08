

using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using FourKeyMetrics.Services.FourKeyService;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Service;

public class ChangeService
{
    private readonly IMongoCollection<Change> _changes;
    public ChangeService()
    {
        _changes = ChangeDb.Open();
    }
    
    public void InsertAllChangeData(List<Change> changes)
    {

        try
        {
            _changes.InsertMany(changes, new InsertManyOptions
            {
                IsOrdered = false
            });
        }
        catch (MongoBulkWriteException){}
    } 
    
    public async Task<IAsyncCursor<Change>> GetChanges(int intervalMonths, string? organization, string? project, string? repository)
    {;
        var builder = Builders<Change>.Filter;
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
        
        
        return await _changes.FindAsync(filter);
        
    }
}

