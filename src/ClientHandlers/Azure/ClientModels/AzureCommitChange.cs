namespace devops_metrics.ClientHandlers.Azure.ClientModels;

using System;
using System.Collections.Generic;

using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

public partial class AzureCommitChangeModel
{
    [J("changeCounts", NullValueHandling = N.Ignore)] public ChangeCounts ChangeCounts { get; set; }
    [J("changes", NullValueHandling = N.Ignore)]      public List<AzureCommitChangeModelChange> Changes { get; set; }     
}

public partial class ChangeCounts
{
    [J("Edit", NullValueHandling = N.Ignore)] public long? Edit { get; set; }
    [J("Add", NullValueHandling = N.Ignore)]  public long? Add { get; set; } 
    [J("Delete", NullValueHandling = N.Ignore)] public long? Delete { get; set; }
}

public partial class AzureCommitChangeModelChange
{
    [J("item", NullValueHandling = N.Ignore)]       public Item Item { get; set; }        
    [J("changeType", NullValueHandling = N.Ignore)] public string ChangeType { get; set; }
}

public partial class Item
{
    [J("objectId", NullValueHandling = N.Ignore)]         public string ObjectId { get; set; }        
    [J("originalObjectId", NullValueHandling = N.Ignore)] public string OriginalObjectId { get; set; }
    [J("gitObjectType", NullValueHandling = N.Ignore)]    public string GitObjectType { get; set; }   
    [J("commitId", NullValueHandling = N.Ignore)]         public string CommitId { get; set; }        
    [J("path", NullValueHandling = N.Ignore)]             public string Path { get; set; }            
    [J("isFolder", NullValueHandling = N.Ignore)]         public bool? IsFolder { get; set; }         
    [J("url", NullValueHandling = N.Ignore)]              public Uri Url { get; set; }                
}