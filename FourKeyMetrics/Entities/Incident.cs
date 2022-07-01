using MongoDB.Driver;

namespace FourKeyMetrics.Entities;

public class Incident {
}

public class IncidentDb
{
    public IMongoCollection<Incident> Open(IConfiguration config)
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_URL"));
        var db = client.GetDatabase("FourKeyMetrics");
        return db.GetCollection<Incident>("incidents");
    }
}