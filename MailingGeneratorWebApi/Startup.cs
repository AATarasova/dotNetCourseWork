using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailing.MailingDal;
using MailingGeneratorBll.Services;
using MailingGeneratorDal.Repository;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.Services;
using MailingsGeneratorBll.Services;
using MailingsGeneratorDal.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MailingGeneratorWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //BLL
            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IControlEventService, ControlEventService>();
            services.AddScoped<ITextService, TextService>();
            
            //DAL
            services.AddScoped<IMailingRepository, MailingRepository>();
            services.AddScoped<ITextRepository, TextRepository>();
            services.AddScoped<IControlEventRepository, ControlEventRepository>();
            services.AddScoped<MailingDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}