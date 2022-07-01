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

    public partial class AzureRepositoryModel
    {
        [J("value", NullValueHandling = N.Ignore)] public List<AzureRepositoryModelValue> Value { get; set; }
        [J("count", NullValueHandling = N.Ignore)] public long? Count { get; set; }      
    }

    public partial class AzureRepositoryModelValue
    {
        [J("id", NullValueHandling = N.Ignore)]            public Guid? Id { get; set; }            
        [J("name", NullValueHandling = N.Ignore)]          public string Name { get; set; }         
        [J("url", NullValueHandling = N.Ignore)]           public Uri Url { get; set; }             
        [J("project", NullValueHandling = N.Ignore)]       public Project Project { get; set; }     
        [J("defaultBranch", NullValueHandling = N.Ignore)] public string DefaultBranch { get; set; }
        [J("size", NullValueHandling = N.Ignore)]          public long? Size { get; set; }          
        [J("remoteUrl", NullValueHandling = N.Ignore)]     public Uri RemoteUrl { get; set; }       
        [J("sshUrl", NullValueHandling = N.Ignore)]        public string SshUrl { get; set; }       
        [J("webUrl", NullValueHandling = N.Ignore)]        public Uri WebUrl { get; set; }          
        [J("isDisabled", NullValueHandling = N.Ignore)]    public bool? IsDisabled { get; set; }    
    }

    public partial class Project
    {
        [J("id", NullValueHandling = N.Ignore)]             public Guid? Id { get; set; }                      
        [J("name", NullValueHandling = N.Ignore)]           public string Name { get; set; }                   
        [J("description", NullValueHandling = N.Ignore)]    public string Description { get; set; }            
        [J("url", NullValueHandling = N.Ignore)]            public Uri Url { get; set; }                       
        [J("state", NullValueHandling = N.Ignore)]          public string State { get; set; }                  
        [J("revision", NullValueHandling = N.Ignore)]       public long? Revision { get; set; }                
        [J("visibility", NullValueHandling = N.Ignore)]     public string Visibility { get; set; }             
        [J("lastUpdateTime", NullValueHandling = N.Ignore)] public DateTimeOffset? LastUpdateTime { get; set; }
    }
}
