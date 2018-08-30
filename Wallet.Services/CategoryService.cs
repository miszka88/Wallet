using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Common;
using Wallet.Common.Helpers;
using Wallet.Domain.Models;
using Wallet.Domain.Repositories;
using Wallet.Domain.Services;

namespace Wallet.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILocalStorage _localStorage;

        private KeyValuePair<string, string> ApiKeyParam { get; }

        public CategoryService(ICategoryRepository categoryRepository, ILocalStorage localStorage)
        {
            _categoryRepository = categoryRepository;
            _localStorage = localStorage;

            ApiKeyParam = new KeyValuePair<string, string>("api_key", _localStorage.ReadVariableValue("ApiKey")?.ToString());
        }

        public async Task<IDictionary<long, string>> GetAll()
        {
            var requiredParam = new KeyValuePair<string, string>("in_wallet", "true");
            var uri = UriBuilderHelper.BuildUri(AccountAction.Categories, ResponseType.Json, ApiKeyParam, requiredParam);

            var response = await GetRawData(uri);
            if (response == null) return null;

            var groupedCategories = JsonConvert.DeserializeObject<Category>(response);

            var allCategories = groupedCategories.CategoryGroups
                .SelectMany(g => g.Categories, (g, c) => c)
                .ToDictionary(d => d.Id, d => d.Name);

            return allCategories;
        }

        public async Task<IDictionary<long, string>> GetByTransactionDirection(TransactionDirection.Type transactionDirectionType)
        {
            var requestParams = new KeyValuePair<string, string>("direction", transactionDirectionType.ToString().ToLower());
            var requiredParam = new KeyValuePair<string, string>("in_wallet", "true");
            var uri = UriBuilderHelper.BuildUri(AccountAction.Categories, ResponseType.Json, ApiKeyParam, requiredParam, requestParams);

            var response = await GetRawData(uri);
            if (response == null) return null;

            var groupedCategories = JsonConvert.DeserializeObject<Category>(response);

            var filteredCategories = groupedCategories.CategoryGroups
                .SelectMany(g => g.Categories, (g, c) => c)
                .ToDictionary(d => d.Id, d => d.Name);

            return filteredCategories;
        }

        private async Task<string> GetRawData(Uri uri)
        {
            var response = await _categoryRepository.Get(uri);

            if (response == null) return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}
