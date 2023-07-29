using Newtonsoft.Json;

namespace AdminManagementWebApi.Test.Models
{
    public class AdminManagementDSLModel
    {
        [JsonProperty(PropertyName = "dslSourceCode")]
        public string? DSLSourceCode { get; set; }
    }
}
