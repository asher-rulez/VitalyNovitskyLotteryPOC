using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.API.BL;
using VitalyNovitskyLotteryPOC.API.DAL;
using VitalyNovitskyLotteryPOC.Common.Interfaces;
using VitalyNovitskyLotteryPOC.Common.Extentions;
using VitalyNovitskyLotteryPOC.Adapters;

namespace VitalyNovitskyLotteryPOC
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerDocumentation();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocumentation("Lottery Game POC");
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(c => AccessInMemoryLotteryData.GetInstance()).As<IHaveAccessToData>().SingleInstance();
            builder.RegisterType<ManageLotteryBL>().As<IManageLotteryBL>();
            builder.RegisterType<LotteryRandomizerWebApi>().As<ILotteryRandomizer>();
        }
    }
}
