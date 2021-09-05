using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VitalyNovitskyLotteryPOC.Common.Interfaces
{
    public interface ILotteryRandomizer
    {
        Task<int?> GetRandomNumber();
    }
}
