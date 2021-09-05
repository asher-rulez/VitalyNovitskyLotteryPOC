using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.Entities;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace VitalyNovitskyLotteryPOC.API.DAL
{
    public class AccessInMemoryLotteryData : IHaveAccessToData
    {
        private static List<LotteryPlayerRecord> LotteryPlayers;
        private static AccessInMemoryLotteryData _instance;

        private AccessInMemoryLotteryData()
        {
            LotteryPlayers = new List<LotteryPlayerRecord>();
        }

        public static AccessInMemoryLotteryData GetInstance()
        {
            _instance = new AccessInMemoryLotteryData();
            return _instance;
        }

        public async Task<PlayLotteryAttemptResultDTO> CreateOrUpdatePlayer(LotteryPlayerRecord player)
        {
            try
            {
                Monitor.Enter(LotteryPlayers);
                var existingPlayer = LotteryPlayers.Where(lp => lp.PlayerName == player.PlayerName).FirstOrDefault();

                if (existingPlayer != null)
                    existingPlayer.Score += player.Score;
                else LotteryPlayers.Add(player);

                return new PlayLotteryAttemptResultDTO
                {
                    IsSuccess = true,
                    ErrorMessage = null,
                    Score = existingPlayer?.Score ?? player.Score,
                    Highest = (existingPlayer?.Score ?? player.Score) == LotteryPlayers.Select(lp => lp.Score).Max()
                };
            }
            catch (Exception ex)
            {
                return new PlayLotteryAttemptResultDTO
                {
                    IsSuccess = false,
                    ErrorMessage = ex.ToString()
                };
            }
            finally
            {
                Monitor.Exit(LotteryPlayers);
            }
        }

        public async Task<IEnumerable<LotteryPlayerRecord>> GetAllPlayers()
        {
            try
            {
                Monitor.Enter(LotteryPlayers);
                return LotteryPlayers;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Monitor.Exit(LotteryPlayers);
            }
        }

        public async Task RemovePlayerByName(string playerName)
        {
            try
            {
                Monitor.Enter(LotteryPlayers);
                var player = LotteryPlayers.Where(lp => lp.PlayerName == playerName).FirstOrDefault();
                if (player != null)
                    LotteryPlayers.Remove(player);
            }
            catch
            {

            }
            finally
            {
                Monitor.Exit(LotteryPlayers);
            }
        }
    }
}
