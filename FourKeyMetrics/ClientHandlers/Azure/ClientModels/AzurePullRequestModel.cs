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

    public partial class AzurePullRequestModel
    {
        [J("value", NullValueHandling = N.Ignore)] public List<AzurePullRequestModelValue> Value { get; set; }
        [J("count", NullValueHandling = N.Ignore)] public long? Count { get; set; }      
    }

    public partial class AzurePullRequestModelValue
    {
        [J("repository", NullValueHandling = N.Ignore)]            public Repository Repository { get; set; }                
        [J("pullRequestId", NullValueHandling = N.Ignore)]         public long? PullRequestId { get; set; }                  
        [J("codeReviewId", NullValueHandling = N.Ignore)]          public long? CodeReviewId { get; set; }                   
        [J("status", NullValueHandling = N.Ignore)]                public string Status { get; set; }                        
        [J("createdBy", NullValueHandling = N.Ignore)]             public CreatedBy CreatedBy { get; set; }                  
        [J("creationDate", NullValueHandling = N.Ignore)]          public DateTimeOffset? CreationDate { get; set; }         
        [J("closedDate", NullValueHandling = N.Ignore)]            public DateTimeOffset? ClosedDate { get; set; }           
        [J("title", NullValueHandling = N.Ignore)]                 public string Title { get; set; }                         
        [J("description", NullValueHandling = N.Ignore)]           public string Description { get; set; }                   
        [J("sourceRefName", NullValueHandling = N.Ignore)]         public string SourceRefName { get; set; }                 
        [J("targetRefName", NullValueHandling = N.Ignore)]         public string TargetRefName { get; set; }                 
        [J("mergeStatus", NullValueHandling = N.Ignore)]           public string MergeStatus { get; set; }                   
        [J("isDraft", NullValueHandling = N.Ignore)]               public bool? IsDraft { get; set; }                        
        [J("mergeId", NullValueHandling = N.Ignore)]               public Guid? MergeId { get; set; }                        
        [J("lastMergeSourceCommit", NullValueHandling = N.Ignore)] public LastMergeCommit LastMergeSourceCommit { get; set; }
        [J("lastMergeTargetCommit", NullValueHandling = N.Ignore)] public LastMergeCommit LastMergeTargetCommit { get; set; }
        [J("lastMergeCommit", NullValueHandling = N.Ignore)]       public LastMergeCommit LastMergeCommit { get; set; }      
        [J("reviewers", NullValueHandling = N.Ignore)]             public List<Reviewer> Reviewers { get; set; }             
        [J("url", NullValueHandling = N.Ignore)]                   public Uri Url { get; set; }                              
        [J("completionOptions", NullValueHandling = N.Ignore)]     public CompletionOptions CompletionOptions { get; set; }  
        [J("supportsIterations", NullValueHandling = N.Ignore)]    public bool? SupportsIterations { get; set; }             
        [J("completionQueueTime", NullValueHandling = N.Ignore)]   public DateTimeOffset? CompletionQueueTime { get; set; }  
    }

    public partial class CompletionOptions
    {
        [J("mergeCommitMessage", NullValueHandling = N.Ignore)]          public string MergeCommitMessage { get; set; }                
        [J("deleteSourceBranch", NullValueHandling = N.Ignore)]          public bool? DeleteSourceBranch { get; set; }                 
        [J("mergeStrategy", NullValueHandling = N.Ignore)]               public string MergeStrategy { get; set; }                     
        [J("transitionWorkItems", NullValueHandling = N.Ignore)]         public bool? TransitionWorkItems { get; set; }                
        [J("autoCompleteIgnoreConfigIds", NullValueHandling = N.Ignore)] public List<dynamic> AutoCompleteIgnoreConfigIds { get; set; }
        [J("squashMerge", NullValueHandling = N.Ignore)]                 public bool? SquashMerge { get; set; }                        
        [J("triggeredByAutoComplete", NullValueHandling = N.Ignore)]     public bool? TriggeredByAutoComplete { get; set; }            
    }

    public partial class CreatedBy
    {
        [J("displayName", NullValueHandling = N.Ignore)] public string DisplayName { get; set; }
        [J("url", NullValueHandling = N.Ignore)]         public Uri Url { get; set; }           
        [J("_links", NullValueHandling = N.Ignore)]      public Links Links { get; set; }       
        [J("id", NullValueHandling = N.Ignore)]          public Guid? Id { get; set; }          
        [J("uniqueName", NullValueHandling = N.Ignore)]  public string UniqueName { get; set; } 
        [J("imageUrl", NullValueHandling = N.Ignore)]    public Uri ImageUrl { get; set; }      
        [J("descriptor", NullValueHandling = N.Ignore)]  public string Descriptor { get; set; } 
    }

    public partial class Links
    {
        [J("avatar", NullValueHandling = N.Ignore)] public Avatar Avatar { get; set; }
    }

    public partial class Avatar
    {
        [J("href", NullValueHandling = N.Ignore)] public Uri Href { get; set; }
    }

    public partial class LastMergeCommit
    {
        [J("commitId", NullValueHandling = N.Ignore)] public string CommitId { get; set; }
        [J("url", NullValueHandling = N.Ignore)]      public Uri Url { get; set; }        
    }

    public partial class Reviewer
    {
        [J("reviewerUrl", NullValueHandling = N.Ignore)] public Uri ReviewerUrl { get; set; }   
        [J("vote", NullValueHandling = N.Ignore)]        public long? Vote { get; set; }        
        [J("hasDeclined", NullValueHandling = N.Ignore)] public bool? HasDeclined { get; set; } 
        [J("isFlagged", NullValueHandling = N.Ignore)]   public bool? IsFlagged { get; set; }   
        [J("displayName", NullValueHandling = N.Ignore)] public string DisplayName { get; set; }
        [J("url", NullValueHandling = N.Ignore)]         public Uri Url { get; set; }           
        [J("_links", NullValueHandling = N.Ignore)]      public Links Links { get; set; }       
        [J("id", NullValueHandling = N.Ignore)]          public Guid? Id { get; set; }          
        [J("uniqueName", NullValueHandling = N.Ignore)]  public string UniqueName { get; set; } 
        [J("imageUrl", NullValueHandling = N.Ignore)]    public Uri ImageUrl { get; set; }      
        [J("isRequired", NullValueHandling = N.Ignore)]  public bool? IsRequired { get; set; }  
    }
}
