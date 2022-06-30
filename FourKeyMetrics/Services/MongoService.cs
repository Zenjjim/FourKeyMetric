using FourKeyMetrics.Models;
using MongoDB.Driver;

namespace FourKeyMetrics.Services;

public class MongoService 
{
    private static MongoClient _client;
    private static DatabaseSettings _settings;

    public MongoService(DatabaseSettings settings)
    {   
        _settings = settings;
        _client = new MongoClient(settings.MongoConnectionString);
    }

    public MongoClient GetClient()
    {
        return _client;
    }

    
}