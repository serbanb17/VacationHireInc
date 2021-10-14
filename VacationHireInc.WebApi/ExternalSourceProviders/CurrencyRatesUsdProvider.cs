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
using System.Threading.Tasks;
using VacationHireInc.WebApi.Interfaces;

namespace VacationHireInc.WebApi.ExternalSourceProviders
{
    public class CurrencyRatesUsdProvider : ICurrencyRatesUsdProvider
    {
        private readonly string _requestUri;
        private readonly uint _cacheLifeTimeSeconds;
        private readonly HttpClient _httpClient;
        private Dictionary<string, decimal> _cachedQuotes;
        private DateTime _cacheExpire;
        private object _lockObj;

        public CurrencyRatesUsdProvider(string apiAccessToken, uint cacheLifeTimeSeconds, HttpClient httpClient)
        {
            _requestUri = @"http://api.currencylayer.com/live?access_key=" + apiAccessToken;
            _cacheLifeTimeSeconds = cacheLifeTimeSeconds;
            _httpClient = httpClient;
            _cacheExpire = DateTime.Now.AddSeconds(-1);
            _lockObj = new object();
        }

        public async Task<Dictionary<string, decimal>> GetRatesAsync()
        {
            await Task.Run(() => UpdateCacheThreadSafe());
            var resultClone = _cachedQuotes?.ToDictionary(kv => new string(kv.Key), kv => kv.Value);
            return resultClone;
        }

        private void UpdateCacheThreadSafe()
        {
            if (_cacheExpire < DateTime.Now)
                lock (_lockObj)
                    if (_cacheExpire < DateTime.Now)
                        //cannot use await inside lock
                        Task.Run(async () => await UpdateCache()).Wait();
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
            _cacheExpire = DateTime.Now.AddSeconds(_cacheLifeTimeSeconds);
        }
    }
}
