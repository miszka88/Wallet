using Newtonsoft.Json;

namespace Wallet.Domain.Models
{
    public class JsonObject
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("user_account")]
        public UserAccount UserAccount { get; set; }
    }
}
