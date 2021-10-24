// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VacationHireInc.WebApi.ExternalSourceProviders;
using VacationHireInc.WebApi.Interfaces;

namespace VacationHireInc.Tests.WebApi
{
    [TestClass]
    public class CurrencyRatesUsdProviderTests
    {
        private const string ResponseSuccessContent = "{\"success\":true,\"terms\":\"https:\\/\\/currencylayer.com\\/terms\",\"privacy\":\"https:\\/\\/currencylayer.com\\/privacy\",\"timestamp\":1633766955,\"source\":\"USD\",\"quotes\":{\"USDAED\":3.672904,\"USDAFN\":90.662356,\"USDALL\":104.889665,\"USDAMD\":478.83623,\"USDANG\":1.794058,\"USDAOA\":598.388041,\"USDARS\":98.806151,\"USDAUD\":1.368176,\"USDAWG\":1.8005,\"USDAZN\":1.70397,\"USDBAM\":1.690534,\"USDBBD\":2.018013,\"USDBDT\":85.518398,\"USDBGN\":1.690509,\"USDBHD\":0.376773,\"USDBIF\":1987.103801,\"USDBMD\":1,\"USDBND\":1.355225,\"USDBOB\":6.906209,\"USDBRL\":5.515718,\"USDBSD\":0.999455,\"USDBTC\":1.819046e-5,\"USDBTN\":75.006612,\"USDBWP\":11.267838,\"USDBYN\":2.466787,\"USDBYR\":19600,\"USDBZD\":2.014642,\"USDCAD\":1.24715,\"USDCDF\":2015.000362,\"USDCHF\":0.92745,\"USDCLF\":0.029879,\"USDCLP\":824.450396,\"USDCNY\":6.443804,\"USDCOP\":3772.484074,\"USDCRC\":625.837345,\"USDCUC\":1,\"USDCUP\":26.5,\"USDCVE\":95.308273,\"USDCZK\":21.96804,\"USDDJF\":177.927792,\"USDDKK\":6.429104,\"USDDOP\":56.239358,\"USDDZD\":137.39804,\"USDEGP\":15.699308,\"USDERN\":15.004954,\"USDETB\":46.492701,\"USDEUR\":0.863904,\"USDFJD\":2.10135,\"USDFKP\":0.72248,\"USDGBP\":0.734565,\"USDGEL\":3.12504,\"USDGGP\":0.72248,\"USDGHS\":6.056719,\"USDGIP\":0.72248,\"USDGMD\":51.503853,\"USDGNF\":9752.707597,\"USDGTQ\":7.735818,\"USDGYD\":208.93226,\"USDHKD\":7.784225,\"USDHNL\":24.080109,\"USDHRK\":6.494904,\"USDHTG\":100.944742,\"USDHUF\":311.220388,\"USDIDR\":14256.85,\"USDILS\":3.23425,\"USDIMP\":0.72248,\"USDINR\":75.131504,\"USDIQD\":1459.206694,\"USDIRR\":42197.503818,\"USDISK\":129.080386,\"USDJEP\":0.72248,\"USDJMD\":148.414338,\"USDJOD\":0.70904,\"USDJPY\":112.236504,\"USDKES\":110.68967,\"USDKGS\":84.803801,\"USDKHR\":4077.774801,\"USDKMF\":425.650384,\"USDKPW\":900.000014,\"USDKRW\":1196.840384,\"USDKWD\":0.30154,\"USDKYD\":0.832894,\"USDKZT\":425.220195,\"USDLAK\":10049.614065,\"USDLBP\":1511.413828,\"USDLKR\":199.891091,\"USDLRD\":170.503775,\"USDLSL\":14.940382,\"USDLTL\":2.95274,\"USDLVL\":0.60489,\"USDLYD\":4.55585,\"USDMAD\":9.064593,\"USDMDL\":17.395521,\"USDMGA\":3948.847381,\"USDMKD\":53.257328,\"USDMMK\":1960.432438,\"USDMNT\":2839.237786,\"USDMOP\":8.014573,\"USDMRO\":356.999828,\"USDMUR\":42.59549,\"USDMVR\":15.450378,\"USDMWK\":815.056226,\"USDMXN\":20.711104,\"USDMYR\":4.178039,\"USDMZN\":63.830377,\"USDNAD\":14.940377,\"USDNGN\":410.810377,\"USDNIO\":35.186226,\"USDNOK\":8.538745,\"USDNPR\":120.010718,\"USDNZD\":1.44321,\"USDOMR\":0.384976,\"USDPAB\":0.999455,\"USDPEN\":4.090308,\"USDPGK\":3.509288,\"USDPHP\":50.593381,\"USDPKR\":170.806963,\"USDPLN\":3.976404,\"USDPYG\":6898.775207,\"USDQAR\":3.64075,\"USDRON\":4.276504,\"USDRSD\":101.60511,\"USDRUB\":71.813104,\"USDRWF\":1016.50921,\"USDSAR\":3.750356,\"USDSBD\":8.067802,\"USDSCR\":13.619666,\"USDSDG\":441.503678,\"USDSEK\":8.737135,\"USDSGD\":1.355504,\"USDSHP\":1.377404,\"USDSLL\":10595.000339,\"USDSOS\":584.000338,\"USDSRD\":21.399038,\"USDSTD\":20697.981008,\"USDSVC\":8.745559,\"USDSYP\":1257.438219,\"USDSZL\":14.898395,\"USDTHB\":33.85037,\"USDTJS\":11.316329,\"USDTMT\":3.51,\"USDTND\":2.831504,\"USDTOP\":2.263804,\"USDTRY\":8.970368,\"USDTTD\":6.79341,\"USDTWD\":28.073704,\"USDTZS\":2303.743528,\"USDUAH\":26.33193,\"USDUGX\":3579.041083,\"USDUSD\":1,\"USDUYU\":43.291297,\"USDUZS\":10674.552479,\"USDVEF\":213830222338.07285,\"USDVND\":22745.542081,\"USDVUV\":111.631732,\"USDWST\":2.560319,\"USDXAF\":566.980716,\"USDXAG\":0.044119,\"USDXAU\":0.000569,\"USDXCD\":2.70255,\"USDXDR\":0.708427,\"USDXOF\":566.980716,\"USDXPF\":103.550364,\"USDYER\":250.250364,\"USDZAR\":14.93135,\"USDZMK\":9001.203593,\"USDZMW\":17.115642,\"USDZWL\":321.999592}}";
        private const string ResponseSuccessContent2 = "{\"success\":true,\"terms\":\"https:\\/\\/currencylayer.com\\/terms\",\"privacy\":\"https:\\/\\/currencylayer.com\\/privacy\",\"timestamp\":1633766955,\"source\":\"USD\",\"quotes\":{\"USDAED\":3.672904,\"USDAFN\":90.662356,\"USDALL\":104.889665,\"USDAMD\":478.83623,\"USDANG\":1.794058,\"USDAOA\":598.388041,\"USDARS\":98.806151,\"USDAUD\":1.368176,\"USDAWG\":1.8005,\"USDAZN\":1.70397,\"USDBAM\":1.690534,\"USDBBD\":2.018013,\"USDBDT\":85.518398,\"USDBGN\":1.690509,\"USDBHD\":0.376773,\"USDBIF\":1987.103801,\"USDBMD\":1,\"USDBND\":1.355225,\"USDBOB\":6.906209,\"USDBRL\":5.515718,\"USDBSD\":0.999455,\"USDBTC\":1.819046e-5,\"USDBTN\":75.006612,\"USDBWP\":11.267838,\"USDBYN\":2.466787,\"USDBYR\":19600,\"USDBZD\":2.014642,\"USDCAD\":1.24715,\"USDCDF\":2015.000362,\"USDCHF\":0.92745,\"USDCLF\":0.029879,\"USDCLP\":824.450396,\"USDCNY\":6.443804,\"USDCOP\":3772.484074,\"USDCRC\":625.837345,\"USDCUC\":1,\"USDCUP\":26.5,\"USDCVE\":95.308273,\"USDCZK\":21.96804,\"USDDJF\":177.927792,\"USDDKK\":6.429104,\"USDDOP\":56.239358,\"USDDZD\":137.39804,\"USDEGP\":15.699308,\"USDERN\":15.004954}}";
        private const string ResponseValuesMerged = "USDAED_3,672904__USDAFN_90,662356__USDALL_104,889665__USDAMD_478,83623__USDANG_1,794058__USDAOA_598,388041__USDARS_98,806151__USDAUD_1,368176__USDAWG_1,8005__USDAZN_1,70397__USDBAM_1,690534__USDBBD_2,018013__USDBDT_85,518398__USDBGN_1,690509__USDBHD_0,376773__USDBIF_1987,103801__USDBMD_1__USDBND_1,355225__USDBOB_6,906209__USDBRL_5,515718__USDBSD_0,999455__USDBTN_75,006612__USDBWP_11,267838__USDBYN_2,466787__USDBYR_19600__USDBZD_2,014642__USDCAD_1,24715__USDCDF_2015,000362__USDCHF_0,92745__USDCLF_0,029879__USDCLP_824,450396__USDCNY_6,443804__USDCOP_3772,484074__USDCRC_625,837345__USDCUC_1__USDCUP_26,5__USDCVE_95,308273__USDCZK_21,96804__USDDJF_177,927792__USDDKK_6,429104__USDDOP_56,239358__USDDZD_137,39804__USDEGP_15,699308__USDERN_15,004954__USDETB_46,492701__USDEUR_0,863904__USDFJD_2,10135__USDFKP_0,72248__USDGBP_0,734565__USDGEL_3,12504__USDGGP_0,72248__USDGHS_6,056719__USDGIP_0,72248__USDGMD_51,503853__USDGNF_9752,707597__USDGTQ_7,735818__USDGYD_208,93226__USDHKD_7,784225__USDHNL_24,080109__USDHRK_6,494904__USDHTG_100,944742__USDHUF_311,220388__USDIDR_14256,85__USDILS_3,23425__USDIMP_0,72248__USDINR_75,131504__USDIQD_1459,206694__USDIRR_42197,503818__USDISK_129,080386__USDJEP_0,72248__USDJMD_148,414338__USDJOD_0,70904__USDJPY_112,236504__USDKES_110,68967__USDKGS_84,803801__USDKHR_4077,774801__USDKMF_425,650384__USDKPW_900,000014__USDKRW_1196,840384__USDKWD_0,30154__USDKYD_0,832894__USDKZT_425,220195__USDLAK_10049,614065__USDLBP_1511,413828__USDLKR_199,891091__USDLRD_170,503775__USDLSL_14,940382__USDLTL_2,95274__USDLVL_0,60489__USDLYD_4,55585__USDMAD_9,064593__USDMDL_17,395521__USDMGA_3948,847381__USDMKD_53,257328__USDMMK_1960,432438__USDMNT_2839,237786__USDMOP_8,014573__USDMRO_356,999828__USDMUR_42,59549__USDMVR_15,450378__USDMWK_815,056226__USDMXN_20,711104__USDMYR_4,178039__USDMZN_63,830377__USDNAD_14,940377__USDNGN_410,810377__USDNIO_35,186226__USDNOK_8,538745__USDNPR_120,010718__USDNZD_1,44321__USDOMR_0,384976__USDPAB_0,999455__USDPEN_4,090308__USDPGK_3,509288__USDPHP_50,593381__USDPKR_170,806963__USDPLN_3,976404__USDPYG_6898,775207__USDQAR_3,64075__USDRON_4,276504__USDRSD_101,60511__USDRUB_71,813104__USDRWF_1016,50921__USDSAR_3,750356__USDSBD_8,067802__USDSCR_13,619666__USDSDG_441,503678__USDSEK_8,737135__USDSGD_1,355504__USDSHP_1,377404__USDSLL_10595,000339__USDSOS_584,000338__USDSRD_21,399038__USDSTD_20697,981008__USDSVC_8,745559__USDSYP_1257,438219__USDSZL_14,898395__USDTHB_33,85037__USDTJS_11,316329__USDTMT_3,51__USDTND_2,831504__USDTOP_2,263804__USDTRY_8,970368__USDTTD_6,79341__USDTWD_28,073704__USDTZS_2303,743528__USDUAH_26,33193__USDUGX_3579,041083__USDUSD_1__USDUYU_43,291297__USDUZS_10674,552479__USDVEF_213830222338,07285__USDVND_22745,542081__USDVUV_111,631732__USDWST_2,560319__USDXAF_566,980716__USDXAG_0,044119__USDXAU_0,000569__USDXCD_2,70255__USDXDR_0,708427__USDXOF_566,980716__USDXPF_103,550364__USDYER_250,250364__USDZAR_14,93135__USDZMK_9001,203593__USDZMW_17,115642__USDZWL_321,999592";
        private const string ResponseValuesMerged2 = "USDAED_3,672904__USDAFN_90,662356__USDALL_104,889665__USDAMD_478,83623__USDANG_1,794058__USDAOA_598,388041__USDARS_98,806151__USDAUD_1,368176__USDAWG_1,8005__USDAZN_1,70397__USDBAM_1,690534__USDBBD_2,018013__USDBDT_85,518398__USDBGN_1,690509__USDBHD_0,376773__USDBIF_1987,103801__USDBMD_1__USDBND_1,355225__USDBOB_6,906209__USDBRL_5,515718__USDBSD_0,999455__USDBTN_75,006612__USDBWP_11,267838__USDBYN_2,466787__USDBYR_19600__USDBZD_2,014642__USDCAD_1,24715__USDCDF_2015,000362__USDCHF_0,92745__USDCLF_0,029879__USDCLP_824,450396__USDCNY_6,443804__USDCOP_3772,484074__USDCRC_625,837345__USDCUC_1__USDCUP_26,5__USDCVE_95,308273__USDCZK_21,96804__USDDJF_177,927792__USDDKK_6,429104__USDDOP_56,239358__USDDZD_137,39804__USDEGP_15,699308__USDERN_15,004954";
        private uint _cacheLifeTimeSeconds;
        private HttpResponseMessage _mockResponse;
        private Mock<HttpMessageHandler> _mockMessageHandler;
        private ICurrencyRatesUsdProvider _currencyRatesUsdProviderSut;
        private Dictionary<string, decimal> _responseValues;
        private Dictionary<string, decimal> _responseValues2;

        [TestInitialize]
        public void Initialize()
        {
            _cacheLifeTimeSeconds = 5;
            _mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(_mockResponse);
            _currencyRatesUsdProviderSut = new CurrencyRatesUsdProvider(string.Empty, _cacheLifeTimeSeconds, new HttpClient(_mockMessageHandler.Object), new ObjectCache<Dictionary<string, decimal>>());

            _responseValues = new Dictionary<string, decimal>();
            string[] kvMerged = ResponseValuesMerged.Split("__");
            foreach (string kvStr in kvMerged)
            {
                string[] kv = kvStr.Split("_");
                _responseValues.Add(kv[0], decimal.Parse(kv[1]));
            }

            _responseValues2 = new Dictionary<string, decimal>();
            string[] kvMerged2 = ResponseValuesMerged2.Split("__");
            foreach (string kvStr in kvMerged2)
            {
                string[] kv = kvStr.Split("_");
                _responseValues2.Add(kv[0], decimal.Parse(kv[1]));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockResponse = null;
            _mockMessageHandler = null;
            _currencyRatesUsdProviderSut = null;
            _responseValues = null;
            _responseValues2 = null;
        }

        [TestMethod]
        public async Task GetRatesAsync_ReturnNull()
        {
            //arrange
            _mockResponse.StatusCode = HttpStatusCode.BadRequest;

            //act
            var rates = await _currencyRatesUsdProviderSut.GetRatesAsync();
            bool responseIsNull = rates is null;

            //assert
            Assert.IsTrue(responseIsNull, "Response should be null!");
        }

        [TestMethod]
        public async Task GetRatesAsync_ReturnCorrectValues()
        {
            //arrange
            _mockResponse.StatusCode = HttpStatusCode.OK;
            _mockResponse.Content = new StringContent(ResponseSuccessContent);

            //act
            var rates = await _currencyRatesUsdProviderSut.GetRatesAsync();
            bool responseIsNull = rates is null;
            bool correctValues = true;
            if(!responseIsNull)
            {
                correctValues = rates.Count == _responseValues.Count;
                foreach (var kv in _responseValues)
                    correctValues &= rates.ContainsKey(kv.Key) && rates[kv.Key] == kv.Value;
            }

            //assert
            Assert.IsFalse(responseIsNull, "Response should not be null!");
            Assert.IsTrue(correctValues, "Response should contain expected values!");
        }

        [TestMethod]
        public async Task GetRatesAsync_ReturnRefreshedCache()
        {
            //arrange
            _mockResponse.StatusCode = HttpStatusCode.OK;
            _mockResponse.Content = new StringContent(ResponseSuccessContent);

            //act
            var rates = await _currencyRatesUsdProviderSut.GetRatesAsync();
            bool responseIsNull = rates is null;
            bool correctValues = true;
            if (!responseIsNull)
            {
                correctValues = rates.Count == _responseValues.Count;
                foreach (var kv in _responseValues)
                    correctValues &= rates.ContainsKey(kv.Key) && rates[kv.Key] == kv.Value;
            }

            await Task.Delay(((int)_cacheLifeTimeSeconds + 1) * 1000);

            //assert
            Assert.IsFalse(responseIsNull, "First response should not be null!");
            Assert.IsTrue(correctValues, "First response should contain expected values!");

            //arrange
            _mockResponse.StatusCode = HttpStatusCode.OK;
            _mockResponse.Content = new StringContent(ResponseSuccessContent2);

            //act
            var rates2 = await _currencyRatesUsdProviderSut.GetRatesAsync();
            bool response2IsNull = rates2 is null;
            bool correctValues2 = true;
            if (!response2IsNull)
            {
                correctValues2 = rates2.Count == _responseValues2.Count;
                foreach (var kv in _responseValues2)
                    correctValues2 &= rates2.ContainsKey(kv.Key) && rates2[kv.Key] == kv.Value;
            }

            //assert
            Assert.IsFalse(response2IsNull, "Second response should not be null!");
            Assert.IsTrue(correctValues2, "Second response should contain expected values!");
        }

        [TestMethod]
        public async Task GetRatesAsync_ReturnCache()
        {
            //arrange
            _mockResponse.StatusCode = HttpStatusCode.OK;
            _mockResponse.Content = new StringContent(ResponseSuccessContent);

            //act
            var rates = await _currencyRatesUsdProviderSut.GetRatesAsync();
            bool responseIsNull = rates is null;
            bool correctValues = true;
            if (!responseIsNull)
            {
                correctValues = rates.Count == _responseValues.Count;
                foreach (var kv in _responseValues)
                    correctValues &= rates.ContainsKey(kv.Key) && rates[kv.Key] == kv.Value;
            }

            //assert
            Assert.IsFalse(responseIsNull, "First response should not be null!");
            Assert.IsTrue(correctValues, "First response should contain expected values!");

            //arrange
            _mockResponse.StatusCode = HttpStatusCode.OK;
            _mockResponse.Content = new StringContent(ResponseSuccessContent2);

            //act
            var rates2 = await _currencyRatesUsdProviderSut.GetRatesAsync();
            bool response2IsNull = rates2 is null;
            bool correctValues2 = true;
            if (!response2IsNull)
            {
                correctValues2 = rates2.Count == _responseValues.Count;
                foreach (var kv in _responseValues)
                    correctValues2 &= rates2.ContainsKey(kv.Key) && rates2[kv.Key] == kv.Value;
            }

            //assert
            Assert.IsFalse(response2IsNull, "Second response should not be null!");
            Assert.IsTrue(correctValues2, "Second response should contain expected values!");
        }
    }
}
