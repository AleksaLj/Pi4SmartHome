using Newtonsoft.Json;

namespace AdminManagementDSL.Application.Common.Models
{
    public class Paged<T>
    {
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "pages")]
        public int Pages { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "rows")]
        public IList<T>? Rows { get; set; }

        [JsonProperty(PropertyName = "total")]
        public long Total { get; set; }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < Pages;
    }
}
