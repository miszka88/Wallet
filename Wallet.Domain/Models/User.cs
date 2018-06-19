using Newtonsoft.Json;

namespace Wallet.Domain.Models
{
    public class UserClass
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }
    public class User
    {
        [JsonProperty("Api_key")]
        public string ApiKey { get; set; }
    }
}
