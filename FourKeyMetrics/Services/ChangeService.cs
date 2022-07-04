

using FourKeyMetrics.Entities;
using FourKeyMetrics.ClientHandlers.Azure;
using FourKeyMetrics.ClientHandlers.Azure.ClientModels;
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
}

