using System.Globalization;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Entities;

public class Deployment
{
    public Deployment(long startTime, long finishTime, string repository, string definition, string project, string organization, string developer, string platform)
    {
        _id = ObjectId.GenerateNewId();
        StartTime = startTime;
        FinishTime = finishTime;
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

    public String Repository { get; set;}

    public String Definition { get; set;}
    
    public String Project { get; set;}
    
    public String Organization { get; set;}

    public String Developer { get; set;}

    public String Platform { get; set;}


    public DateTime GetStartDateTime() => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(this.StartTime).ToLocalTime();

    public DateTime GetFinishDateTime() => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(this.FinishTime).ToLocalTime();
    public int GetWeek() => DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(this.GetStartDateTime(), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
}
public static class DeploymentDb
{
    public static IMongoCollection<Deployment> Open()
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_URL"));
        var db = client.GetDatabase(Environment.GetEnvironmentVariable("MONGO_DB"));
        var collectionName = "deployments";
        var uniqueIndexes =
            "{ Platform: 1, Organization: 1, Project: 1, Definition: 1, Repository: 1, StartTime: 1, FinishTime: 1 }";
        var collectionExists = db.ListCollectionNames().ToList().Contains(collectionName);
        if (collectionExists == false) {
            db.CreateCollection(collectionName);
            var collection = db.GetCollection<Deployment>(collectionName);
            var options = new CreateIndexOptions { Unique = true };
            collection.Indexes.CreateOne(uniqueIndexes, options);
        }

        return db.GetCollection<Deployment>(collectionName);
    }
}
