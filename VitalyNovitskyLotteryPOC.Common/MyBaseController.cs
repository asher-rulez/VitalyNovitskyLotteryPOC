using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace VitalyNovitskyLotteryPOC.Common
{
    public class MyBaseController : Controller
    {
        protected readonly IWebHostEnvironment _appEnvironment;
        protected readonly ILifetimeScope Container;

        public MyBaseController(IWebHostEnvironment appEnvironment, ILifetimeScope icoContext)
        {
            _appEnvironment = appEnvironment;
            Container = icoContext;
        }

    }
}
