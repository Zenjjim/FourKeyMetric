namespace FourKeyMetrics.ClientHandlers.Azure.ClientModels
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using R = Newtonsoft.Json.Required;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class AzureBuildModel
    {
        [J("count", NullValueHandling = N.Ignore)] public long? Count { get; set; }      
        [J("value", NullValueHandling = N.Ignore)] public List<AzureBuildModelValue> Value { get; set; }
    }

    public partial class AzureBuildModelValue
    {
        [J("_links", NullValueHandling = N.Ignore)]              public ValueLinks Links { get; set; }               
        [J("properties", NullValueHandling = N.Ignore)]          public Properties Properties { get; set; }          
        [J("tags", NullValueHandling = N.Ignore)]                public List<dynamic> Tags { get; set; }              
        [J("validationResults", NullValueHandling = N.Ignore)]   public List<dynamic> ValidationResults { get; set; } 
        [J("plans", NullValueHandling = N.Ignore)]               public List<Plan> Plans { get; set; }               
        [J("triggerInfo", NullValueHandling = N.Ignore)]         public TriggerInfo TriggerInfo { get; set; }        
        [J("id", NullValueHandling = N.Ignore)]                  public long? Id { get; set; }                       
        [J("buildNumber", NullValueHandling = N.Ignore)]         public string BuildNumber { get; set; }             
        [J("status", NullValueHandling = N.Ignore)]              public string Status { get; set; }                  
        [J("result", NullValueHandling = N.Ignore)]              public string Result { get; set; }                  
        [J("queueTime", NullValueHandling = N.Ignore)]           public DateTimeOffset? QueueTime { get; set; }      
        [J("startTime", NullValueHandling = N.Ignore)]           public DateTimeOffset? StartTime { get; set; }      
        [J("finishTime", NullValueHandling = N.Ignore)]          public DateTimeOffset? FinishTime { get; set; }     
        [J("url", NullValueHandling = N.Ignore)]                 public Uri Url { get; set; }                        
        [J("definition", NullValueHandling = N.Ignore)]          public Definition Definition { get; set; }          
        [J("buildNumberRevision", NullValueHandling = N.Ignore)] public long? BuildNumberRevision { get; set; }      
        [J("project", NullValueHandling = N.Ignore)]             public Project Project { get; set; }                
        [J("uri", NullValueHandling = N.Ignore)]                 public string Uri { get; set; }                     
        [J("sourceBranch", NullValueHandling = N.Ignore)]        public string SourceBranch { get; set; }            
        [J("sourceVersion", NullValueHandling = N.Ignore)]       public string SourceVersion { get; set; }           
        [J("queue", NullValueHandling = N.Ignore)]               public Queue Queue { get; set; }                    
        [J("priority", NullValueHandling = N.Ignore)]            public string Priority { get; set; }                
        [J("reason", NullValueHandling = N.Ignore)]              public string Reason { get; set; }                  
        [J("requestedFor", NullValueHandling = N.Ignore)]        public LastChangedBy RequestedFor { get; set; }     
        [J("requestedBy", NullValueHandling = N.Ignore)]         public LastChangedBy RequestedBy { get; set; }      
        [J("lastChangedDate", NullValueHandling = N.Ignore)]     public DateTimeOffset? LastChangedDate { get; set; }
        [J("lastChangedBy", NullValueHandling = N.Ignore)]       public LastChangedBy LastChangedBy { get; set; }    
        [J("orchestrationPlan", NullValueHandling = N.Ignore)]   public Plan OrchestrationPlan { get; set; }         
        [J("logs", NullValueHandling = N.Ignore)]                public Logs Logs { get; set; }                      
        [J("repository", NullValueHandling = N.Ignore)]          public Repository Repository { get; set; }          
        [J("keepForever", NullValueHandling = N.Ignore)]         public bool? KeepForever { get; set; }              
        [J("retainedByRelease", NullValueHandling = N.Ignore)]   public bool? RetainedByRelease { get; set; }        
        [J("triggeredByBuild")]                                  public dynamic TriggeredByBuild { get; set; }        
    }

    public partial class Definition
    {
        [J("drafts", NullValueHandling = N.Ignore)]      public List<dynamic> Drafts { get; set; }
        [J("id", NullValueHandling = N.Ignore)]          public long? Id { get; set; }           
        [J("name", NullValueHandling = N.Ignore)]        public string Name { get; set; }        
        [J("url", NullValueHandling = N.Ignore)]         public Uri Url { get; set; }            
        [J("uri", NullValueHandling = N.Ignore)]         public string Uri { get; set; }         
        [J("path", NullValueHandling = N.Ignore)]        public string Path { get; set; }        
        [J("type", NullValueHandling = N.Ignore)]        public string Type { get; set; }        
        [J("queueStatus", NullValueHandling = N.Ignore)] public string QueueStatus { get; set; } 
        [J("revision", NullValueHandling = N.Ignore)]    public long? Revision { get; set; }     
        [J("project", NullValueHandling = N.Ignore)]     public Project Project { get; set; }    
    }
    
    public partial class LastChangedBy
    {
        [J("displayName", NullValueHandling = N.Ignore)] public string DisplayName { get; set; }      
        [J("url", NullValueHandling = N.Ignore)]         public Uri Url { get; set; }                 
        [J("_links", NullValueHandling = N.Ignore)]      public LastChangedByLinks Links { get; set; }
        [J("id", NullValueHandling = N.Ignore)]          public Guid? Id { get; set; }                
        [J("uniqueName", NullValueHandling = N.Ignore)]  public string UniqueName { get; set; }       
        [J("imageUrl", NullValueHandling = N.Ignore)]    public Uri ImageUrl { get; set; }            
        [J("descriptor", NullValueHandling = N.Ignore)]  public string Descriptor { get; set; }       
    }

    public partial class LastChangedByLinks
    {
        [J("avatar", NullValueHandling = N.Ignore)] public Badge Avatar { get; set; }
    }

    public partial class Badge
    {
        [J("href", NullValueHandling = N.Ignore)] public Uri Href { get; set; }
    }

    public partial class ValueLinks
    {
        [J("self", NullValueHandling = N.Ignore)]                    public Badge Self { get; set; }                   
        [J("web", NullValueHandling = N.Ignore)]                     public Badge Web { get; set; }                    
        [J("sourceVersionDisplayUri", NullValueHandling = N.Ignore)] public Badge SourceVersionDisplayUri { get; set; }
        [J("timeline", NullValueHandling = N.Ignore)]                public Badge Timeline { get; set; }               
        [J("badge", NullValueHandling = N.Ignore)]                   public Badge Badge { get; set; }                  
    }

    public partial class Logs
    {
        [J("id", NullValueHandling = N.Ignore)]   public long? Id { get; set; }   
        [J("type", NullValueHandling = N.Ignore)] public string Type { get; set; }
        [J("url", NullValueHandling = N.Ignore)]  public Uri Url { get; set; }    
    }

    public partial class Plan
    {
        [J("planId", NullValueHandling = N.Ignore)] public Guid? PlanId { get; set; }
    }

    public partial class Properties
    {
    }

    public partial class Queue
    {
        [J("id", NullValueHandling = N.Ignore)]   public long? Id { get; set; }   
        [J("name", NullValueHandling = N.Ignore)] public string Name { get; set; }
        [J("pool", NullValueHandling = N.Ignore)] public Pool Pool { get; set; }  
    }

    public partial class Pool
    {
        [J("id", NullValueHandling = N.Ignore)]       public long? Id { get; set; }      
        [J("name", NullValueHandling = N.Ignore)]     public string Name { get; set; }   
        [J("isHosted", NullValueHandling = N.Ignore)] public bool? IsHosted { get; set; }
    }

    public partial class Repository
    {
        [J("id", NullValueHandling = N.Ignore)]                 public Guid? Id { get; set; }                
        [J("type", NullValueHandling = N.Ignore)]               public string Type { get; set; }             
        [J("name", NullValueHandling = N.Ignore)]               public string Name { get; set; }             
        [J("url", NullValueHandling = N.Ignore)]                public Uri Url { get; set; }                 
        [J("clean")]                                            public dynamic Clean { get; set; }            
        [J("checkoutSubmodules", NullValueHandling = N.Ignore)] public bool? CheckoutSubmodules { get; set; }
    }

    public partial class TriggerInfo
    {
        [J("ci.sourceBranch", NullValueHandling = N.Ignore)]      public string CiSourceBranch { get; set; }    
        [J("ci.sourceSha", NullValueHandling = N.Ignore)]         public string CiSourceSha { get; set; }       
        [J("ci.message", NullValueHandling = N.Ignore)]           public string CiMessage { get; set; }         
        [J("ci.triggerRepository", NullValueHandling = N.Ignore)] public Guid? CiTriggerRepository { get; set; }
    }
}
