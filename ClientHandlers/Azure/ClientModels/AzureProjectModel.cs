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

    public partial class AzureProjectModel
    {
        [J("count", NullValueHandling = N.Ignore)] public long? Count { get; set; }      
        [J("value", NullValueHandling = N.Ignore)] public List<AzureProjectModelValue> Value { get; set; }
    }

    public partial class AzureProjectModelValue
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