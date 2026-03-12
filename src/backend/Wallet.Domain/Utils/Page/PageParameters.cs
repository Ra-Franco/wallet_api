using System.Text.Json.Serialization;

namespace Wallet.Domain.Utils.Page
{
    public class PageParameters
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; } = 1;
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 20;
    }
}
