using Newtonsoft.Json;

namespace Pi4SmartHomeWebApi.Test.Models
{
    public class ResultModel : HttpResponseMessage
    {
        public ResultModel()
        {
            StatusCode = System.Net.HttpStatusCode.OK;
        }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string? Message { get; set; }

        [JsonProperty(PropertyName = "exception")]
        public Exception? Exception { get; set; }
    }
}
