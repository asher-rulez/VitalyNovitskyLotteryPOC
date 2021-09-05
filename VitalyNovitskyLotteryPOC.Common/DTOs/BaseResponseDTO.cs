using System;
using System.Collections.Generic;
using System.Text;

namespace VitalyNovitskyLotteryPOC.Common.DTOs
{
    public class BaseResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static BaseResponseDTO GetFailureResponse(string message)
        {
            return new BaseResponseDTO
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}
