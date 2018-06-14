﻿using Newtonsoft.Json;
using System;

namespace Wallet.Domain.Models
{
    public class MoneyTransactionClass
    {
        [JsonProperty("money_transaction")]
        public MoneyTransaction MoneyTransaction { get; set; }
    }

    public class MoneyTransaction
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("user_account_id")]
        public long UserAccountId { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("transaction_on")]
        public DateTimeOffset TransactionOn { get; set; }

        [JsonProperty("booked_on")]
        public DateTimeOffset BookedOn { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("party")]
        public string Party { get; set; }

        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        [JsonProperty("excluded_from_calculations")]
        public bool ExcludedFromCalculations { get; set; }

        [JsonProperty("currency_amount")]
        public string CurrencyAmount { get; set; }

        [JsonProperty("removed")]
        public bool Removed { get; set; }

        [JsonProperty("plain_party_iban")]
        public object PlainPartyIban { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("currency_name")]
        public string CurrencyName { get; set; }

        [JsonProperty("tag_string")]
        public string TagString { get; set; }
    }
}
