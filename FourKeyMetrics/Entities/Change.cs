using MongoDB.Bson;
using MongoDB.Driver;

namespace FourKeyMetrics.Entities;

public class Change {
    public Change(long startTime, long finishTimeTime, bool isInProduction, bool isFix, long prSize, long nrOfCommits, string pullRequestId, string branch, string repository, string project, string organization, string developer, string platform)
    {
        _id = ObjectId.GenerateNewId();
        StartTime = startTime;
        FinishTimeTime = finishTimeTime;
        IsInProduction = isInProduction;
        IsFix = isFix;
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
    
    public long FinishTimeTime { get; set; } // Build finish time
    
    public Boolean IsFix { get; set;} // Discussion is needed
    
    public Boolean IsInProduction { get; set;}
    
    public long PrSize { get; set;} // Sum of all edits in all commits
    
    public long NrOfCommits { get; set;} // Count nr of commits
    
    public String PullRequestId { get; set; }

    public String Branch { get; set; }
    
    public String Repository { get; set;}

    public String Project { get; set;}
    
    public String Organization { get; set;}

    public String Developer { get; set;}

    public String Platform { get; set;}
}

public static class ChangeDb
{
    public static IMongoCollection<Change> Open()
    {

        var client = new MongoClient(Environment.GetEnvironmentVariable("MONGO_URL"));
        var db = client.GetDatabase("FourKeyMetrics");
        return db.GetCollection<Change>("changes");
    }
}