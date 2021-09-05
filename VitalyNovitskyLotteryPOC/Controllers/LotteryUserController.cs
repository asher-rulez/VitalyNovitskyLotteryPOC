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
using VitalyNovitskyLotteryPOC.Common;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace VitalyNovitskyLotteryPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotteryUserController : MyBaseController
    {
        public LotteryUserController(IWebHostEnvironment appEnvironment, ILifetimeScope container)
            : base(appEnvironment, container)
                { }


        [HttpPost("PlayLottery")]
        public async Task<IActionResult> PlayLottery([FromBody]PlayLotteryAttemptDTO playLotteryAttemptDTO, CancellationToken cancellationToken)
        {
            if(playLotteryAttemptDTO == null)
                return StatusCode((int)HttpStatusCode.BadRequest, BaseResponseDTO.GetFailureResponse("Invalid request DTO"));

            var inputValidationResult = playLotteryAttemptDTO.Validate();

            if(!string.IsNullOrEmpty(inputValidationResult))
                return StatusCode((int)HttpStatusCode.BadRequest, BaseResponseDTO.GetFailureResponse(inputValidationResult));

            try
            {
                await Container.Resolve<IManageLotteryBL>().AttemptPlayLottery(playLotteryAttemptDTO);

                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }


}
