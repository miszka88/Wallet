using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wallet.Domain.Models
{
    public class Category
    {
        [JsonProperty("category_groups")]
        public IEnumerable<CategoryGroup> CategoryGroups { get; set; }
    }

    public class CategoryGroup
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("categories")]
        public IEnumerable<CategoryElement> Categories { get; set; }
    }

    public class CategoryElement
    {
        [JsonProperty("category_group_id")]
        public long CategoryGroupId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("spending")]
        public bool Spending { get; set; }
    }
}
