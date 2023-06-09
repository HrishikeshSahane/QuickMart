using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickMartDataAccessLayer.Models;
using AutoMapper;
using QuickMartCoreMVCWeb.Repository;
using Microsoft.EntityFrameworkCore;

namespace QuickMartCoreMVCWeb
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
            services.AddMvc();
            services.AddSession();
            services.AddControllersWithViews();
            services.AddAutoMapper(x => x.AddProfile(new QuickMartMapper()));
            var connection = Configuration.GetConnectionString("QuickMartCon");
            services.AddDbContext<QuickMartDBContext>(options => options.UseSqlServer(connection));
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Login}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Register}/{id?}");
            });


            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=FestiveHours}/{id?}");
            });




            app.UseAuthorization();
        }
    }
}
