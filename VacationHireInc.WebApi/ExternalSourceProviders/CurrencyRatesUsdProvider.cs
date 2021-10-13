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
                        Task.Run(async () => await UpdateCache()).Wait();
        }

        private async Task UpdateCache()
        {
            Dictionary<string, decimal> newCache = null;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_requestUri);
                //HttpResponseMessage response = await Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //string responseBody = "{\"success\":true,\"terms\":\"https:\\/\\/currencylayer.com\\/terms\",\"privacy\":\"https:\\/\\/currencylayer.com\\/privacy\",\"timestamp\":1633766955,\"source\":\"USD\",\"quotes\":{\"USDAED\":3.672904,\"USDAFN\":90.662356,\"USDALL\":104.889665,\"USDAMD\":478.83623,\"USDANG\":1.794058,\"USDAOA\":598.388041,\"USDARS\":98.806151,\"USDAUD\":1.368176,\"USDAWG\":1.8005,\"USDAZN\":1.70397,\"USDBAM\":1.690534,\"USDBBD\":2.018013,\"USDBDT\":85.518398,\"USDBGN\":1.690509,\"USDBHD\":0.376773,\"USDBIF\":1987.103801,\"USDBMD\":1,\"USDBND\":1.355225,\"USDBOB\":6.906209,\"USDBRL\":5.515718,\"USDBSD\":0.999455,\"USDBTC\":1.819046e-5,\"USDBTN\":75.006612,\"USDBWP\":11.267838,\"USDBYN\":2.466787,\"USDBYR\":19600,\"USDBZD\":2.014642,\"USDCAD\":1.24715,\"USDCDF\":2015.000362,\"USDCHF\":0.92745,\"USDCLF\":0.029879,\"USDCLP\":824.450396,\"USDCNY\":6.443804,\"USDCOP\":3772.484074,\"USDCRC\":625.837345,\"USDCUC\":1,\"USDCUP\":26.5,\"USDCVE\":95.308273,\"USDCZK\":21.96804,\"USDDJF\":177.927792,\"USDDKK\":6.429104,\"USDDOP\":56.239358,\"USDDZD\":137.39804,\"USDEGP\":15.699308,\"USDERN\":15.004954,\"USDETB\":46.492701,\"USDEUR\":0.863904,\"USDFJD\":2.10135,\"USDFKP\":0.72248,\"USDGBP\":0.734565,\"USDGEL\":3.12504,\"USDGGP\":0.72248,\"USDGHS\":6.056719,\"USDGIP\":0.72248,\"USDGMD\":51.503853,\"USDGNF\":9752.707597,\"USDGTQ\":7.735818,\"USDGYD\":208.93226,\"USDHKD\":7.784225,\"USDHNL\":24.080109,\"USDHRK\":6.494904,\"USDHTG\":100.944742,\"USDHUF\":311.220388,\"USDIDR\":14256.85,\"USDILS\":3.23425,\"USDIMP\":0.72248,\"USDINR\":75.131504,\"USDIQD\":1459.206694,\"USDIRR\":42197.503818,\"USDISK\":129.080386,\"USDJEP\":0.72248,\"USDJMD\":148.414338,\"USDJOD\":0.70904,\"USDJPY\":112.236504,\"USDKES\":110.68967,\"USDKGS\":84.803801,\"USDKHR\":4077.774801,\"USDKMF\":425.650384,\"USDKPW\":900.000014,\"USDKRW\":1196.840384,\"USDKWD\":0.30154,\"USDKYD\":0.832894,\"USDKZT\":425.220195,\"USDLAK\":10049.614065,\"USDLBP\":1511.413828,\"USDLKR\":199.891091,\"USDLRD\":170.503775,\"USDLSL\":14.940382,\"USDLTL\":2.95274,\"USDLVL\":0.60489,\"USDLYD\":4.55585,\"USDMAD\":9.064593,\"USDMDL\":17.395521,\"USDMGA\":3948.847381,\"USDMKD\":53.257328,\"USDMMK\":1960.432438,\"USDMNT\":2839.237786,\"USDMOP\":8.014573,\"USDMRO\":356.999828,\"USDMUR\":42.59549,\"USDMVR\":15.450378,\"USDMWK\":815.056226,\"USDMXN\":20.711104,\"USDMYR\":4.178039,\"USDMZN\":63.830377,\"USDNAD\":14.940377,\"USDNGN\":410.810377,\"USDNIO\":35.186226,\"USDNOK\":8.538745,\"USDNPR\":120.010718,\"USDNZD\":1.44321,\"USDOMR\":0.384976,\"USDPAB\":0.999455,\"USDPEN\":4.090308,\"USDPGK\":3.509288,\"USDPHP\":50.593381,\"USDPKR\":170.806963,\"USDPLN\":3.976404,\"USDPYG\":6898.775207,\"USDQAR\":3.64075,\"USDRON\":4.276504,\"USDRSD\":101.60511,\"USDRUB\":71.813104,\"USDRWF\":1016.50921,\"USDSAR\":3.750356,\"USDSBD\":8.067802,\"USDSCR\":13.619666,\"USDSDG\":441.503678,\"USDSEK\":8.737135,\"USDSGD\":1.355504,\"USDSHP\":1.377404,\"USDSLL\":10595.000339,\"USDSOS\":584.000338,\"USDSRD\":21.399038,\"USDSTD\":20697.981008,\"USDSVC\":8.745559,\"USDSYP\":1257.438219,\"USDSZL\":14.898395,\"USDTHB\":33.85037,\"USDTJS\":11.316329,\"USDTMT\":3.51,\"USDTND\":2.831504,\"USDTOP\":2.263804,\"USDTRY\":8.970368,\"USDTTD\":6.79341,\"USDTWD\":28.073704,\"USDTZS\":2303.743528,\"USDUAH\":26.33193,\"USDUGX\":3579.041083,\"USDUSD\":1,\"USDUYU\":43.291297,\"USDUZS\":10674.552479,\"USDVEF\":213830222338.07285,\"USDVND\":22745.542081,\"USDVUV\":111.631732,\"USDWST\":2.560319,\"USDXAF\":566.980716,\"USDXAG\":0.044119,\"USDXAU\":0.000569,\"USDXCD\":2.70255,\"USDXDR\":0.708427,\"USDXOF\":566.980716,\"USDXPF\":103.550364,\"USDYER\":250.250364,\"USDZAR\":14.93135,\"USDZMK\":9001.203593,\"USDZMW\":17.115642,\"USDZWL\":321.999592}}";
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
