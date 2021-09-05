using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.Entities;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace VitalyNovitskyLotteryPOC.API.BL
{
    public interface IManageLotteryBL
    {
        Task<LotteryPlayerWithMagicNumberDTO> GetWinner();
        Task<PlayLotteryAttemptResultDTO> AttemptPlayLottery(PlayLotteryAttemptRequestDTO playLotteryAttemptDTO);
        Task<LotteryPlayerWithMagicNumberDTO> RemoveWinnerAndGetNextOne();
        Task<List<LotteryPlayerRecord>> GetAllPlayers();
    }
}
