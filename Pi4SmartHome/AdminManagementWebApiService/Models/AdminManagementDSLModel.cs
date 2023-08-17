using Newtonsoft.Json;

namespace AdminManagementWebApiService.Models
{
    public class AdminManagementDSLModel
    {
        [JsonProperty(PropertyName = "dslSourceCode")]
        public string? DSLSourceCode { get; set; }
    }
}
