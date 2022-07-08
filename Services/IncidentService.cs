using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Service;

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

