using System.Collections.Generic;
using Core.Data.Repository;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Production;
using ServiceBusMessaging;
using ServiceBusMessaging.MessagingModels;

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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<BikeStoresContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(@"Server=.\SQLExpress;Database=BikeStores;Trusted_Connection=True;");
            });
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IProcess, ProductionBusinessLogic>();
            services.AddScoped(typeof(IQueueReceiver<>), typeof(QueueReceiver<>));

            services.AddCors();
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

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default",
                                                 "{controller=Home}/{action=Index}/{id?}");

            });

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    var bus = scope.ServiceProvider.GetService<IQueueReceiver<List<UpdateStocksDto>>>();
            //    bus.ReceiveMessages().GetAwaiter().GetResult();
            //}
        }
    }
}
