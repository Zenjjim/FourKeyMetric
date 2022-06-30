using MongoDB.Driver;

namespace FourKeyMetrics.Entity;

public class Changes {
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
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