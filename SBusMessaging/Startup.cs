using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Core.Data.Repository;
using Core.Domain;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceBusMessaging;

namespace SBusMessaging
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
            //services.AddControllers().AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});
            services.AddDbContext<BikeStoresContext>(options =>
            {
                options.UseSqlServer(@"Server=.\SQLExpress;Database=BikeStores;Trusted_Connection=True;");
            });
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddCors();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
         .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
         .AddNewtonsoftJson(options =>
         {
             options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
         });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());


           // app.UseRouting();
            app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute("default",
            //                                     "{controller=Home}/{action=Index}/{id?}");

            //});

        }
    }
}
