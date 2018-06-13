using Newtonsoft.Json;

namespace Wallet.Domain.Models
{
    public class UserAccount
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("iban_checksum")]
        public string IbanChecksum { get; set; }

        [JsonProperty("currency_balance")]
        public string CurrencyBalance { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("plain_iban")]
        public string PlainIban { get; set; }

        [JsonProperty("currency_funds_available")]
        public string CurrencyFundsAvailable { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("bank_plugin_name")]
        public string BankPluginName { get; set; }

        [JsonProperty("bank_position")]
        public long BankPosition { get; set; }

        [JsonProperty("currency_name")]
        public string CurrencyName { get; set; }

        [JsonProperty("is_default_wallet")]
        public bool IsDefaultWallet { get; set; }
    }

    public class UserAccountClass
    {
        [JsonProperty("user_account")]
        public UserAccount UserAccount { get; set; }
    }
}
