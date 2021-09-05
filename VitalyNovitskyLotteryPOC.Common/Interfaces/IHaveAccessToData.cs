using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.Entities;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace VitalyNovitskyLotteryPOC.API.DAL
{
    public interface IHaveAccessToData
    {
        Task<IEnumerable<LotteryPlayerRecord>> GetAllPlayers();
        Task CreateOrUpdatePlayer(LotteryPlayerRecord player);
        Task RemovePlayerByName(string playerName);
    }
}
