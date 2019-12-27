using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Utility.Email;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OdeToFood.Core;
using OdeToFood.Data;
using OdeToFood.Infrastructure;

namespace OdeToFood
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
            services.AddDbContextPool<OdeToFoodDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("OdeToFoodDb"));
                options.UseSqlite(Configuration.GetConnectionString("OdeToFoodSqlite"));
            });
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddTransient<IEmailSender, IdentityEmailSender>();
            services.AddScoped<IRestaurantData, SqlRestaurantData>();
            //services.AddDefaultIdentity<OdeToFoodUser>
            services.AddIdentity<OdeToFoodUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;//必須要驗證Email

                options.Password.RequiredLength = 3;//最少密碼長度
                options.Password.RequireDigit = false;//需要數字
                options.Password.RequireNonAlphanumeric = false;//需要非英數的字元 ex:@
                options.Password.RequireUppercase = false;//需要大寫字元
                options.Password.RequiredUniqueChars = 3; //至少要有個字元不一樣

                options.User.RequireUniqueEmail = true;//Email不能重覆

                options.Lockout.MaxFailedAccessAttempts = 3;//3次密碼誤就鎖定網站
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);//鎖定1分鐘
                options.Lockout.AllowedForNewUsers = true;//新增的使用者也會被鎖定
            })
            .AddEntityFrameworkStores<OdeToFoodDbContext>()
            //.AddDefaultUI()
            .AddDefaultTokenProviders();

            services.Configure<CookieAuthenticationOptions>(IdentityConstants.TwoFactorRememberMeScheme, o =>
            {
                o.Cookie.Name = "SuccessReName";
                o.ExpireTimeSpan = TimeSpan.FromDays(365);
            });

            services.AddRazorPages();
            services.AddControllersWithViews();
            /*
            services.AddDbContext<OdeToFoodContext>(options =>
                     options.UseSqlite(
                         context.Configuration.GetConnectionString("OdeToFoodContextConnection")));

            services.AddDefaultIdentity<OdeToFoodUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<OdeToFoodContext>();
                */
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(SayHelloMiddleware);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseNodeModules();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
        private RequestDelegate SayHelloMiddleware(
                                  RequestDelegate next)
        {
            return async ctx =>
                         {

                             if (ctx.Request.Path.StartsWithSegments("/hello"))
                             {
                                 await ctx.Response.WriteAsync("Hello, World!");
                             }
                             else
                             {
                                 await next(ctx);
                             }
                         };
        }
    }
}
