using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Entities;

public class Deployment 
{
    public Deployment(long startTime, long finishTime, string result, string repository, string definition, string project, string organization, string developer, string platform)
    {
        _id = ObjectId.GenerateNewId();
        StartTime = startTime;
        FinishTime = finishTime;
        Result = result;
        Repository = repository;
        Definition = definition;
        Project = project;
        Organization = organization;
        Developer = developer;
        Platform = platform;
    }
    
    public ObjectId _id { get; set; }
    public long StartTime { get; set; }

    public long FinishTime { get; set; }

    public String Result { get; set;}

    public String Repository { get; set;}

    public String Definition { get; set;}
    
    public String Project { get; set;}
    
    public String Organization { get; set;}

    public String Developer { get; set;}

    public String Platform { get; set;}
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
