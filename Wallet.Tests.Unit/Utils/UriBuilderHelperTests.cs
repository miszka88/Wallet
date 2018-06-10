using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Wallet.Common;
using Wallet.Services.Utils;

namespace Wallet.Tests.Unit.Utils
{
    [TestClass]
    public class UriBuilderHelperTests
    {
        [TestMethod]
        public void should_build_uri_for_api_key()
        {
            // arrange
            var action = AccountAction.Session;
            var format = ResponseType.Json;

            var expectedUri = new Uri(@"https://secure.kontomierz.pl/k4/session.json");

            // act
            var uri = UriBuilderHelper.BuildUri(action, format);

            // assert
            Assert.AreEqual(expectedUri, uri);
        }

        [TestMethod]
        public void should_build_uri_with_query()
        {
            // arrange
            var action = AccountAction.UserAccounts;
            var format = ResponseType.Json;
            KeyValuePair<string, string>[] queryParams = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("start_on", "01-01-2001"),
                new KeyValuePair<string, string>("api_key", "some-random-api-key")
            };

            var expectedUri = new Uri(@"https://secure.kontomierz.pl/k4/user_accounts.json?start_on=01-01-2001&api_key=some-random-api-key");

            // act
            var uri = UriBuilderHelper.BuildUri(action, format, queryParams);

            // assert
            Assert.AreEqual(expectedUri, uri);
        }
    }
}
