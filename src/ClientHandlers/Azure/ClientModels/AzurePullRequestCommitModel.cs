namespace devops_metrics.ClientHandlers.Azure.ClientModels;

using System;
using System.Collections.Generic;

using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

public partial class AzurePullRequestCommitModel
{
    [J("count", NullValueHandling = N.Ignore)] public long? Count { get; set; }      
    [J("value", NullValueHandling = N.Ignore)] public List<AzurePullRequestCommitModelValue> Value { get; set; }
}

public partial class AzurePullRequestCommitModelValue
{
    [J("commitId", NullValueHandling = N.Ignore)]  public string CommitId { get; set; } 
    [J("author", NullValueHandling = N.Ignore)]    public Author Author { get; set; }   
    [J("committer", NullValueHandling = N.Ignore)] public Author Committer { get; set; }
    [J("comment", NullValueHandling = N.Ignore)]   public string Commit { get; set; }  
    [J("url", NullValueHandling = N.Ignore)]       public Uri Url { get; set; }         
}

public partial class Author
{
    [J("name", NullValueHandling = N.Ignore)]  public string Name { get; set; }         
    [J("email", NullValueHandling = N.Ignore)] public string Email { get; set; }        
    [J("date", NullValueHandling = N.Ignore)]  public DateTimeOffset? Date { get; set; }
}