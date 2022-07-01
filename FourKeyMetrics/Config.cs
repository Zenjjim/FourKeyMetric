namespace FourKeyMetrics
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using R = Newtonsoft.Json.Required;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class Config
    {
        [J("Platform", NullValueHandling = N.Ignore)]     public string Platform { get; set; }    
        [J("Organization", NullValueHandling = N.Ignore)] public string Organization { get; set; }
    }
}