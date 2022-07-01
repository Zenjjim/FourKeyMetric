using MongoDB.Driver;

namespace FourKeyMetrics.Entities;

public class Changes {
    
}

public class ChangeDb
{
    public IMongoCollection<Changes> Open(IConfiguration config)
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_URL"));
        var db = client.GetDatabase("FourKeyMetrics");
        return db.GetCollection<Changes>("changes");
    }
}