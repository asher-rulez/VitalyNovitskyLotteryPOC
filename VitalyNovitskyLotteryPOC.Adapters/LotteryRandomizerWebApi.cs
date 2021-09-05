using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.Common.Interfaces;

namespace VitalyNovitskyLotteryPOC.Adapters
{
    public class LotteryRandomizerWebApi : ILotteryRandomizer
    {
        public async Task<int?> GetRandomNumber()
        {
            List<int?> result = null;

            var httpResponse = await new HttpClient().GetAsync("http://www.randomnumberapi.com/api/v1.0/random?min=1&max=1000000");

            if (httpResponse.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<List<int?>>(await httpResponse.Content.ReadAsStringAsync());
            }

            return result.FirstOrDefault();
        }
    }
}
