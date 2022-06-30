using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Entity;

public class Deployment {
    
    public ObjectId? _id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime FinishTime { get; set; }

    public String Result { get; set;}

    public String Repository { get; set;}

    public String Project { get; set;}
    
    
}

public static class DeploymentDb
{
    public static IMongoCollection<Deployment> Open()
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_URL"));
        var db = client.GetDatabase("FourKeyMetrics");
        return db.GetCollection<Deployment>("deployments");
    }
}