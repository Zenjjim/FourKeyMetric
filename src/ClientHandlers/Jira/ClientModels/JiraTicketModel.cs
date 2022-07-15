namespace devops_metrics.ClientHandlers.Jira.ClientModels;

using System;
using System.Collections.Generic;

using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

public partial class JiraTicketModel
{
    [J("expand", NullValueHandling = N.Ignore)] public string? Expand { get; set; }       
    [J("id", NullValueHandling = N.Ignore)]     public string? Id { get; set; }           
    [J("self", NullValueHandling = N.Ignore)]   public Uri? Self { get; set; }            
    [J("key", NullValueHandling = N.Ignore)]    public string? Key { get; set; }          
    [J("fields", NullValueHandling = N.Ignore)] public WelcomeFields? Fields { get; set; }
}

public partial class WelcomeFields
{
    [J("statuscategorychangedate", NullValueHandling = N.Ignore)] public string? Statuscategorychangedate { get; set; }      
    [J("issuetype", NullValueHandling = N.Ignore)]                public Issuetype? Issuetype { get; set; }                  
    [J("parent", NullValueHandling = N.Ignore)]                   public Parent? Parent { get; set; }                        
    [J("timespent")]                                              public dynamic? Timespent { get; set; }                    
    [J("customfield_10073")]                                      public dynamic? Customfield10073 { get; set; }             
    [J("project", NullValueHandling = N.Ignore)]                  public Project? Project { get; set; }                      
    [J("fixVersions", NullValueHandling = N.Ignore)]              public List<dynamic>? FixVersions { get; set; }            
    [J("aggregatetimespent")]                                     public dynamic? Aggregatetimespent { get; set; }           
    [J("resolution")]                                             public dynamic? Resolution { get; set; }                   
    [J("resolutiondate")]                                         public dynamic? Resolutiondate { get; set; }               
    [J("workratio", NullValueHandling = N.Ignore)]                public long? Workratio { get; set; }                      
    [J("watches", NullValueHandling = N.Ignore)]                  public Watches? Watches { get; set; }                      
    [J("lastViewed", NullValueHandling = N.Ignore)]               public string? LastViewed { get; set; }                    
    [J("issuerestriction", NullValueHandling = N.Ignore)]         public Issuerestriction? Issuerestriction { get; set; }    
    [J("customfield_10060")]                                      public dynamic? Customfield10060 { get; set; }             
    [J("created", NullValueHandling = N.Ignore)]                  public DateTimeOffset Created { get; set; }                       
    [J("customfield_10061")]                                      public dynamic? Customfield10061 { get; set; }             
    [J("customfield_10065", NullValueHandling = N.Ignore)]        public string? Customfield10065 { get; set; }              
    [J("customfield_10066")]                                      public dynamic? Customfield10066 { get; set; }             
    [J("priority", NullValueHandling = N.Ignore)]                 public Priority? Priority { get; set; }                    
    [J("customfield_10025")]                                      public dynamic? Customfield10025 { get; set; }             
    [J("labels", NullValueHandling = N.Ignore)]                   public List<dynamic>? Labels { get; set; }                 
    [J("customfield_10018")]                                      public dynamic? Customfield10018 { get; set; }             
    [J("timeestimate")]                                           public dynamic? Timeestimate { get; set; }                 
    [J("aggregatetimeoriginalestimate")]                          public dynamic? Aggregatetimeoriginalestimate { get; set; }
    [J("versions", NullValueHandling = N.Ignore)]                 public List<dynamic>? Versions { get; set; }               
    [J("issuelinks", NullValueHandling = N.Ignore)]               public List<dynamic>? Issuelinks { get; set; }             
    [J("assignee", NullValueHandling = N.Ignore)]                 public Assignee? Assignee { get; set; }                    
    [J("updated", NullValueHandling = N.Ignore)]                  public string? Updated { get; set; }                       
    [J("status", NullValueHandling = N.Ignore)]                   public Status? Status { get; set; }                        
    [J("components", NullValueHandling = N.Ignore)]               public List<dynamic>? Components { get; set; }             
    [J("customfield_10050")]                                      public dynamic? Customfield10050 { get; set; }             
    [J("customfield_10095")]                                      public dynamic? Customfield10095 { get; set; }             
    [J("timeoriginalestimate")]                                   public dynamic? Timeoriginalestimate { get; set; }         
    [J("customfield_10096")]                                      public dynamic? Customfield10096 { get; set; }             
    [J("customfield_10053")]                                      public dynamic? Customfield10053 { get; set; }             
    [J("description")]                                            public dynamic? Description { get; set; }                  
    [J("customfield_10097")]                                      public dynamic? Customfield10097 { get; set; }             
    [J("customfield_10010")]                                      public dynamic? Customfield10010 { get; set; }             
    [J("customfield_10054")]                                      public dynamic? Customfield10054 { get; set; }             
    [J("customfield_10055")]                                      public dynamic? Customfield10055 { get; set; }             
    [J("customfield_10011", NullValueHandling = N.Ignore)]        public string? Customfield10011 { get; set; }              
    [J("customfield_10012")]                                      public dynamic? Customfield10012 { get; set; }             
    [J("customfield_10056", NullValueHandling = N.Ignore)]        public List<dynamic>? Customfield10056 { get; set; }       
    [J("customfield_10013")]                                      public dynamic? Customfield10013 { get; set; }             
    [J("customfield_10059")]                                      public dynamic? Customfield10059 { get; set; }             
    [J("timetracking", NullValueHandling = N.Ignore)]             public Timetracking? Timetracking { get; set; }            
    [J("customfield_10049")]                                      public dynamic? Customfield10049 { get; set; }             
    [J("security")]                                               public dynamic? Security { get; set; }                     
    [J("customfield_10008")]                                      public dynamic? Customfield10008 { get; set; }             
    [J("customfield_10009", NullValueHandling = N.Ignore)]        public Customfield10009? Customfield10009 { get; set; }    
    [J("aggregatetimeestimate")]                                  public dynamic? Aggregatetimeestimate { get; set; }        
    [J("attachment", NullValueHandling = N.Ignore)]               public List<dynamic>? Attachment { get; set; }             
    [J("summary", NullValueHandling = N.Ignore)]                  public string? Summary { get; set; }                       
    [J("creator", NullValueHandling = N.Ignore)]                  public Assignee? Creator { get; set; }                     
    [J("customfield_10082")]                                      public dynamic? Customfield10082 { get; set; }             
    [J("subtasks", NullValueHandling = N.Ignore)]                 public List<dynamic>? Subtasks { get; set; }               
    [J("customfield_10083")]                                      public dynamic? Customfield10083 { get; set; }             
    [J("customfield_10084")]                                      public dynamic? Customfield10084 { get; set; }             
    [J("customfield_10085")]                                      public dynamic? Customfield10085 { get; set; }             
    [J("customfield_10086")]                                      public dynamic? Customfield10086 { get; set; }             
    [J("customfield_10042")]                                      public dynamic? Customfield10042 { get; set; }             
    [J("customfield_10043")]                                      public dynamic? Customfield10043 { get; set; }             
    [J("reporter", NullValueHandling = N.Ignore)]                 public Assignee? Reporter { get; set; }                    
    [J("customfield_10087")]                                      public dynamic? Customfield10087 { get; set; }             
    [J("customfield_10044")]                                      public dynamic? Customfield10044 { get; set; }             
    [J("customfield_10000", NullValueHandling = N.Ignore)]        public string? Customfield10000 { get; set; }              
    [J("aggregateprogress", NullValueHandling = N.Ignore)]        public Progress? Aggregateprogress { get; set; }           
    [J("customfield_10045")]                                      public dynamic? Customfield10045 { get; set; }             
    [J("customfield_10001")]                                      public dynamic? Customfield10001 { get; set; }             
    [J("customfield_10046")]                                      public dynamic? Customfield10046 { get; set; }             
    [J("customfield_10047")]                                      public dynamic? Customfield10047 { get; set; }             
    [J("customfield_10004")]                                      public dynamic? Customfield10004 { get; set; }             
    [J("customfield_10048")]                                      public dynamic? Customfield10048 { get; set; }             
    [J("customfield_10039")]                                      public dynamic? Customfield10039 { get; set; }             
    [J("environment")]                                            public dynamic? Environment { get; set; }                  
    [J("duedate")]                                                public dynamic? Duedate { get; set; }                      
    [J("progress", NullValueHandling = N.Ignore)]                 public Progress? Progress { get; set; }                    
    [J("votes", NullValueHandling = N.Ignore)]                    public Votes? Votes { get; set; }                          
    [J("comment", NullValueHandling = N.Ignore)]                  public Comment? Comment { get; set; }                      
    [J("worklog", NullValueHandling = N.Ignore)]                  public Worklog? Worklog { get; set; }                      
}

public partial class Progress
{
    [J("progress", NullValueHandling = N.Ignore)] public long? ProgressProgress { get; set; }
    [J("total", NullValueHandling = N.Ignore)]    public long? Total { get; set; }           
}

public partial class Assignee
{
    [J("self", NullValueHandling = N.Ignore)]         public Uri? Self { get; set; }             
    [J("accountId", NullValueHandling = N.Ignore)]    public string? AccountId { get; set; }     
    [J("emailAddress", NullValueHandling = N.Ignore)] public string? EmailAddress { get; set; }  
    [J("avatarUrls", NullValueHandling = N.Ignore)]   public AvatarUrls? AvatarUrls { get; set; }
    [J("displayName", NullValueHandling = N.Ignore)]  public string? DisplayName { get; set; }   
    [J("active", NullValueHandling = N.Ignore)]       public bool? Active { get; set; }         
    [J("timeZone", NullValueHandling = N.Ignore)]     public string? TimeZone { get; set; }      
    [J("accountType", NullValueHandling = N.Ignore)]  public string? AccountType { get; set; }   
}

public partial class AvatarUrls
{
    [J("48x48", NullValueHandling = N.Ignore)] public Uri? The48X48 { get; set; }
    [J("24x24", NullValueHandling = N.Ignore)] public Uri? The24X24 { get; set; }
    [J("16x16", NullValueHandling = N.Ignore)] public Uri? The16X16 { get; set; }
    [J("32x32", NullValueHandling = N.Ignore)] public Uri? The32X32 { get; set; }
}

public partial class Comment
{
    [J("comments", NullValueHandling = N.Ignore)]   public List<dynamic>? Comments { get; set; }
    [J("self", NullValueHandling = N.Ignore)]       public Uri? Self { get; set; }              
    [J("maxResults", NullValueHandling = N.Ignore)] public long? MaxResults { get; set; }      
    [J("total", NullValueHandling = N.Ignore)]      public long? Total { get; set; }           
    [J("startAt", NullValueHandling = N.Ignore)]    public long? StartAt { get; set; }         
}

public partial class Customfield10009
{
    [J("hasEpicLinkFieldDependency", NullValueHandling = N.Ignore)] public bool? HasEpicLinkFieldDependency { get; set; }   
    [J("showField", NullValueHandling = N.Ignore)]                  public bool? ShowField { get; set; }                    
    [J("nonEditableReason", NullValueHandling = N.Ignore)]          public NonEditableReason? NonEditableReason { get; set; }
}

public partial class NonEditableReason
{
    [J("reason", NullValueHandling = N.Ignore)]  public string? Reason { get; set; } 
    [J("message", NullValueHandling = N.Ignore)] public string? Message { get; set; }
}

public partial class Issuerestriction
{
    [J("issuerestrictions", NullValueHandling = N.Ignore)] public Timetracking? Issuerestrictions { get; set; }
    [J("shouldDisplay", NullValueHandling = N.Ignore)]     public bool? ShouldDisplay { get; set; }           
}

public partial class Timetracking
{
}

public partial class Issuetype
{
    [J("self", NullValueHandling = N.Ignore)]           public Uri? Self { get; set; }            
    [J("id", NullValueHandling = N.Ignore)]             public string? Id { get; set; }           
    [J("description", NullValueHandling = N.Ignore)]    public string? Description { get; set; }  
    [J("iconUrl", NullValueHandling = N.Ignore)]        public Uri? IconUrl { get; set; }         
    [J("name", NullValueHandling = N.Ignore)]           public string? Name { get; set; }         
    [J("subtask", NullValueHandling = N.Ignore)]        public bool? Subtask { get; set; }       
    [J("avatarId", NullValueHandling = N.Ignore)]       public long? AvatarId { get; set; }      
    [J("entityId", NullValueHandling = N.Ignore)]       public Guid? EntityId { get; set; }      
    [J("hierarchyLevel", NullValueHandling = N.Ignore)] public long? HierarchyLevel { get; set; }
}

public partial class Parent
{
    [J("id", NullValueHandling = N.Ignore)]     public string? Id { get; set; }          
    [J("key", NullValueHandling = N.Ignore)]    public string? Key { get; set; }         
    [J("self", NullValueHandling = N.Ignore)]   public Uri? Self { get; set; }           
    [J("fields", NullValueHandling = N.Ignore)] public ParentFields? Fields { get; set; }
}

public partial class ParentFields
{
    [J("summary", NullValueHandling = N.Ignore)]   public string? Summary { get; set; }     
    [J("status", NullValueHandling = N.Ignore)]    public Status? Status { get; set; }      
    [J("priority", NullValueHandling = N.Ignore)]  public Priority? Priority { get; set; }  
    [J("issuetype", NullValueHandling = N.Ignore)] public Issuetype? Issuetype { get; set; }
}

public partial class Priority
{
    [J("self", NullValueHandling = N.Ignore)]        public Uri? Self { get; set; }          
    [J("iconUrl", NullValueHandling = N.Ignore)]     public Uri? IconUrl { get; set; }       
    [J("name", NullValueHandling = N.Ignore)]        public string? Name { get; set; }       
    [J("id", NullValueHandling = N.Ignore)]          public string? Id { get; set; }         
}

public partial class Status
{
    [J("self", NullValueHandling = N.Ignore)]           public Uri? Self { get; set; }                     
    [J("description", NullValueHandling = N.Ignore)]    public string? Description { get; set; }           
    [J("iconUrl", NullValueHandling = N.Ignore)]        public Uri? IconUrl { get; set; }                  
    [J("name", NullValueHandling = N.Ignore)]           public string? Name { get; set; }                  
    [J("id", NullValueHandling = N.Ignore)]             public string? Id { get; set; }                    
    [J("statusCategory", NullValueHandling = N.Ignore)] public StatusCategory? StatusCategory { get; set; }
}

public partial class StatusCategory
{
    [J("self", NullValueHandling = N.Ignore)]      public Uri? Self { get; set; }        
    [J("id", NullValueHandling = N.Ignore)]        public long? Id { get; set; }        
    [J("key", NullValueHandling = N.Ignore)]       public string? Key { get; set; }      
    [J("colorName", NullValueHandling = N.Ignore)] public string? ColorName { get; set; }
    [J("name", NullValueHandling = N.Ignore)]      public string? Name { get; set; }     
}

public partial class Project
{
    [J("self", NullValueHandling = N.Ignore)]            public Uri? Self { get; set; }                
    [J("id", NullValueHandling = N.Ignore)]              public string? Id { get; set; }               
    [J("key", NullValueHandling = N.Ignore)]             public string? Key { get; set; }              
    [J("name", NullValueHandling = N.Ignore)]            public string? Name { get; set; }             
    [J("projectTypeKey", NullValueHandling = N.Ignore)]  public string? ProjectTypeKey { get; set; }   
    [J("simplified", NullValueHandling = N.Ignore)]      public bool? Simplified { get; set; }        
    [J("avatarUrls", NullValueHandling = N.Ignore)]      public AvatarUrls? AvatarUrls { get; set; }   
    [J("projectCategory", NullValueHandling = N.Ignore)] public Priority? ProjectCategory { get; set; }
}

public partial class Votes
{
    [J("self", NullValueHandling = N.Ignore)]     public Uri? Self { get; set; }        
    [J("votes", NullValueHandling = N.Ignore)]    public long? VotesVotes { get; set; }
    [J("hasVoted", NullValueHandling = N.Ignore)] public bool? HasVoted { get; set; }  
}

public partial class Watches
{
    [J("self", NullValueHandling = N.Ignore)]       public Uri? Self { get; set; }        
    [J("watchCount", NullValueHandling = N.Ignore)] public long? WatchCount { get; set; }
    [J("isWatching", NullValueHandling = N.Ignore)] public bool? IsWatching { get; set; }
}

public partial class Worklog
{
    [J("startAt", NullValueHandling = N.Ignore)]    public long? StartAt { get; set; }         
    [J("maxResults", NullValueHandling = N.Ignore)] public long? MaxResults { get; set; }      
    [J("total", NullValueHandling = N.Ignore)]      public long? Total { get; set; }           
    [J("worklogs", NullValueHandling = N.Ignore)]   public List<dynamic>? Worklogs { get; set; }
}