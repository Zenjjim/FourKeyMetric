namespace devops_metrics.ClientHandlers.Azure.ClientModels;

using System;
using System.Collections.Generic;

using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

public partial class AzureDefinitionModel
{
    [J("count", NullValueHandling = N.Ignore)] public long? Count { get; set; }      
    [J("value", NullValueHandling = N.Ignore)] public List<AzureDefinitionModelValue> Value { get; set; }
}

public partial class AzureDefinitionModelValue
{
    [J("_links", NullValueHandling = N.Ignore)]      public ValueLinks Links { get; set; }           
    [J("quality", NullValueHandling = N.Ignore)]     public string Quality { get; set; }             
    [J("authoredBy", NullValueHandling = N.Ignore)]  public AuthoredBy AuthoredBy { get; set; }      
    [J("drafts", NullValueHandling = N.Ignore)]      public List<dynamic> Drafts { get; set; }       
    [J("queue", NullValueHandling = N.Ignore)]       public Queue Queue { get; set; }                
    [J("id", NullValueHandling = N.Ignore)]          public long? Id { get; set; }                   
    [J("name", NullValueHandling = N.Ignore)]        public string Name { get; set; }                
    [J("url", NullValueHandling = N.Ignore)]         public Uri Url { get; set; }                    
    [J("uri", NullValueHandling = N.Ignore)]         public string Uri { get; set; }                 
    [J("path", NullValueHandling = N.Ignore)]        public string Path { get; set; }                
    [J("type", NullValueHandling = N.Ignore)]        public string Type { get; set; }                
    [J("queueStatus", NullValueHandling = N.Ignore)] public string QueueStatus { get; set; }         
    [J("revision", NullValueHandling = N.Ignore)]    public long? Revision { get; set; }             
    [J("createdDate", NullValueHandling = N.Ignore)] public DateTimeOffset? CreatedDate { get; set; }
    [J("project", NullValueHandling = N.Ignore)]     public Project Project { get; set; }            
}

public partial class AuthoredBy
{
    [J("displayName", NullValueHandling = N.Ignore)] public string DisplayName { get; set; }   
    [J("url", NullValueHandling = N.Ignore)]         public Uri Url { get; set; }              
    [J("_links", NullValueHandling = N.Ignore)]      public AuthoredByLinks Links { get; set; }
    [J("id", NullValueHandling = N.Ignore)]          public Guid? Id { get; set; }             
    [J("uniqueName", NullValueHandling = N.Ignore)]  public string UniqueName { get; set; }    
    [J("imageUrl", NullValueHandling = N.Ignore)]    public Uri ImageUrl { get; set; }         
    [J("descriptor", NullValueHandling = N.Ignore)]  public string Descriptor { get; set; }    
}

public partial class AuthoredByLinks
{
    [J("avatar", NullValueHandling = N.Ignore)] public Badge Avatar { get; set; }
}

public partial class QueueLinks
{
    [J("self", NullValueHandling = N.Ignore)] public Badge Self { get; set; }
}