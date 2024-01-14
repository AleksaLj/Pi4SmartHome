using Newtonsoft.Json;

namespace Pi4SmartHomeWebApi.Test.Models
{
    public class Pi4SmartHomeDslModel
    {
        [JsonProperty(PropertyName = "dslSourceCode")]
        public string? DslSourceCode { get; set; }
    }
}
