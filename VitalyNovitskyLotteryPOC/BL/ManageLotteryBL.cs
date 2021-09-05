using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.DAL;
using VitalyNovitskyLotteryPOC.API.Entities;
using VitalyNovitskyLotteryPOC.Common.Interfaces;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace VitalyNovitskyLotteryPOC.API.BL
{
    public class ManageLotteryBL : IManageLotteryBL
    {
        protected ILifetimeScope Container { get; private set; }

        public ManageLotteryBL(ILifetimeScope container)
        {
            Container = container;
        }



        public async Task<LotteryPlayerWithMagicNumberDTO> GetWinner()
        {
            var allPlayers = await Container.Resolve<IHaveAccessToData>().GetAllPlayers();
            var winner = allPlayers.OrderByDescending(p => p.Score).FirstOrDefault();

            if (winner == null)
                return null;

            return await LotteryPlayerWithMagicNumberDTO.GetPlayerWithCalculatedMagicNumber(winner);
        }

        public async Task<PlayLotteryAttemptResultDTO> AttemptPlayLottery(PlayLotteryAttemptRequestDTO playLotteryAttemptDTO)
        {
            var getTicketsTasks = new List<Task<int?>>();
            for (int i = 0; i < playLotteryAttemptDTO.NumberOfTickets; i++)
                getTicketsTasks.Add(Container.Resolve<ILotteryRandomizer>().GetRandomNumber());

            Task.WaitAll(getTicketsTasks.ToArray());

            return await Container.Resolve<IHaveAccessToData>().CreateOrUpdatePlayer(new LotteryPlayerRecord
            {
                PlayerName = playLotteryAttemptDTO.UserName,
                Score = getTicketsTasks.Where(t => t.Result.HasValue).Sum(t => t.Result.Value)
            });
        }

        public async Task<LotteryPlayerWithMagicNumberDTO> RemoveWinnerAndGetNextOne()
        {
            var winner = await GetWinner();
            if(winner != null)
                await Container.Resolve<IHaveAccessToData>().RemovePlayerByName(winner.PlayerName);

            return await GetWinner();
        }

        public async Task<List<LotteryPlayerRecord>> GetAllPlayers()
        {
            return (await Container.Resolve<IHaveAccessToData>().GetAllPlayers()).ToList();
        }
    }
}
