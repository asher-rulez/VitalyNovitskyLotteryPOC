using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VitalyNovitskyLotteryPOC.Common.Adapters
{
    public interface ILotteryRandomizer
    {
        Task<int?> GetRandomNumber();
    }
}
