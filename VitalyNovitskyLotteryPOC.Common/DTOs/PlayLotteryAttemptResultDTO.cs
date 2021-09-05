using System;
using System.Collections.Generic;
using System.Text;

namespace VitalyNovitskyLotteryPOC.Common.DTOs
{
    public class PlayLotteryAttemptResultDTO : BaseResponseDTO
    {
        public long Score { get; set; }
        public bool Highest { get; set; }
    }
}
