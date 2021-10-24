// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VacationHireInc.WebApi.Interfaces;

namespace VacationHireInc.WebApi.ExternalSourceProviders
{
    public class CurrencyRatesUsdProvider : ICurrencyRatesUsdProvider
    {
        private readonly string _requestUri;
        private readonly uint _cacheLifeTime;
        private readonly HttpClient _httpClient;
        private Dictionary<string, decimal> _cachedQuotes;
        private DateTime _cacheExpire;
        private object _lockObj;
        private SemaphoreSlim _semaphoreSlim;

        /// <summary>
        /// Implements ICurrencyRatesUsdProvider.
        /// Internally calls <see href="https://currencylayer.com/documentation">currencylayer API</see>
        /// </summary>
        /// <param name="apiAccessToken">
        /// token to access the api from currencylayer
        /// </param>
        /// <param name="cacheLifeTime">
        /// Number of seconds to keep last returned data from API in cache memory
        /// </param>
        /// <param name="httpClient">
        /// Client used to make requests to API
        /// </param>
        public CurrencyRatesUsdProvider(string apiAccessToken, uint cacheLifeTime, HttpClient httpClient)
        {
            _requestUri = @"http://api.currencylayer.com/live?access_key=" + apiAccessToken;
            _cacheLifeTime = cacheLifeTime;
            _httpClient = httpClient;
            _cacheExpire = DateTime.Now.AddSeconds(-1);
            _lockObj = new object();
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// Thread-safe. Get USD rates with other currency from external API. 
        /// </summary>
        /// <returns>
        /// Returns cached data that is being refreshed as provided in constructor.
        /// Returns null if API request is not successful, or if parsing the response fails.
        /// </returns>
        public async Task<Dictionary<string, decimal>> GetRatesAsync()
        {
            await Task.Run(() => UpdateCacheThreadSafe());
            var resultClone = _cachedQuotes?.ToDictionary(kv => new string(kv.Key), kv => kv.Value);
            return resultClone;
        }

        private void UpdateCacheThreadSafe()
        {
            if (_cacheExpire < DateTime.Now)
            {
                _semaphoreSlim.Wait(-1);
                if (_cacheExpire < DateTime.Now)
                    Task.Run(async () => await UpdateCache()).Wait();
                _semaphoreSlim.Release();
            }
        }

        private async Task UpdateCache()
        {
            Dictionary<string, decimal> newCache = null;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_requestUri);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic respObj = JsonConvert.DeserializeObject<ExpandoObject>(responseBody, new ExpandoObjectConverter());
                    dynamic quotes = respObj.quotes;
                    JObject parsedResponseBody = JObject.Parse(responseBody);
                    var parsedQuotes = parsedResponseBody["quotes"];
                    if (parsedQuotes != null)
                    {
                        newCache = new Dictionary<string, decimal>();
                        foreach (var q in parsedQuotes)
                        {
                            string name = ((JProperty)q)?.Name?.ToString();
                            string valStr = ((JProperty)q)?.Value?.ToString();
                            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(valStr)
                                && decimal.TryParse(valStr, out decimal val)
                                && !newCache.ContainsKey(name))
                                newCache.Add(name, val);
                        }
                    }
                }
            }
            catch
            {
                newCache = null;
            }

            _cachedQuotes = newCache;
            _cacheExpire = DateTime.Now.AddSeconds(_cacheLifeTime);
        }
    }
}
