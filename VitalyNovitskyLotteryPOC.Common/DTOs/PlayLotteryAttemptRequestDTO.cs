using System;
using System.Collections.Generic;
using System.Text;

namespace VitalyNovitskyLotteryPOC.Common.DTOs
{
    public class PlayLotteryAttemptRequestDTO
    {
        public string UserName { get; set; }
        public int NumberOfTickets { get; set; }

        public string Validate()
        {
            if (string.IsNullOrEmpty(UserName))
                return "UserName empty or null";

            if (NumberOfTickets < 1 || NumberOfTickets > 10)
                return "Number of tickets must be between 1 and 10";

            return null;
        }
    }
}
