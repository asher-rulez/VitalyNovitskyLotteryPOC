using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.Entities;

namespace VitalyNovitskyLotteryPOC.Common.DTOs
{
    public class LotteryPlayerWithMagicNumberDTO
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public string MagicNumber { get; set; }

        public static async Task<LotteryPlayerWithMagicNumberDTO> GetPlayerWithCalculatedMagicNumber(LotteryPlayerRecord winner)
        {
            var magicNumberTask = Task<long>.Run(() => { return BigInteger.Pow(winner.Score, 60000); });
            return new LotteryPlayerWithMagicNumberDTO
            {
                PlayerName = winner.PlayerName,
                Score = winner.Score,
                MagicNumber = (await magicNumberTask).ToString()
            };
        }
    }
}
