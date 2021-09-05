using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.BL;
using VitalyNovitskyLotteryPOC.API.DAL;
using VitalyNovitskyLotteryPOC.Common;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace VitalyNovitskyLotteryPOC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotteryAdministratorController : MyBaseController
    {
        public LotteryAdministratorController(IWebHostEnvironment appEnvironment, ILifetimeScope container)
            : base(appEnvironment, container)
                { }

        [HttpGet("GetWinner")]
        public async Task<IActionResult> GetWinner(CancellationToken cancellationToken)
        {
            try
            {
                var winner = await Container.Resolve<IManageLotteryBL>().GetWinner();

                return Ok(winner) ?? (IActionResult)NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpDelete("RemoveWinner")]
        public async Task<IActionResult> RemoveWinner(CancellationToken cancellationToken)
        {
            try
            {
                var nextWinner = await Container.Resolve<IManageLotteryBL>().RemoveWinnerAndGetNextOne();
                return nextWinner != null
                    ? Ok(nextWinner)
                    : (IActionResult)Accepted();
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("GetAllPlayers")]
        public async Task<IActionResult> GetAllPlayers(CancellationToken cancellationToken)
        {
            try
            {
                var players = await Container.Resolve<IManageLotteryBL>().GetAllPlayers();
                return players == null
                    ? NotFound()
                    : (IActionResult)Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
