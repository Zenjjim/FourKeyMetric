using System.Globalization;
using MongoDB.Bson;
using MongoDB.Driver;

namespace devops_metrics.Entities;

public class Change {
    public Change(long startTime, long finishTime, long prSize, long nrOfCommits, string pullRequestId, string branch, string repository, string project, string organization, string developer, string platform)
    {
        _id = ObjectId.GenerateNewId();
        StartTime = startTime;
        FinishTime = finishTime;
        PrSize = prSize;
        NrOfCommits = nrOfCommits;
        PullRequestId = pullRequestId;
        Branch = branch;
        Repository = repository;
        Project = project;
        Organization = organization;
        Developer = developer;
        Platform = platform;
    }

    public ObjectId _id { get; set; }

    public long StartTime { get; set; } // Pull request start time
    
    public long FinishTime { get; set; } // Build finish time
    
    public long PrSize { get; set;} // Sum of all edits in all commits
    
    public long NrOfCommits { get; set;} // Count nr of commits
    
    public String PullRequestId { get; set; }

    public String Branch { get; set; }
    
    public String Repository { get; set;}

    public String Project { get; set;}
    
    public String Organization { get; set;}

    public String Developer { get; set;}

    public String Platform { get; set;}
    
    public DateTime GetStartDateTime() => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(this.StartTime).ToLocalTime();
    public DateTime GetFinishDateTime() => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(this.FinishTime).ToLocalTime();
    public int GetWeek() => DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(this.GetStartDateTime(), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
    public double Delta() {
        return Utils.CalculateBusinessHours(this.GetStartDateTime(), this.GetFinishDateTime());
    }
}

public static class ChangeDb
{
    public static IMongoCollection<Change> Open()
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_URL"));
        var db = client.GetDatabase(Environment.GetEnvironmentVariable("MONGO_DB"));
        var collectionName = "changes";
        var uniqueIndexes =
            "{ Platform: 1, Organization: 1, Project: 1, Repository: 1, Branch: 1, PullRequestId: 1 }";
        var collectionExists = db.ListCollectionNames().ToList().Contains(collectionName);
        if (collectionExists == false) {
            db.CreateCollection(collectionName);
            var collection = db.GetCollection<Change>(collectionName);
            var options = new CreateIndexOptions { Unique = true };
            collection.Indexes.CreateOne(uniqueIndexes, options);
        }

        return db.GetCollection<Change>(collectionName);
    }
}