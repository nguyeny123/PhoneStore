using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Data.Entities.Orders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ECommerce.Infrastructure;
using Stripe;


namespace test3
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // if (_env.EnvironmentName == EnvironmentName.Development)
            // {
                 //services.AddDbContext<EcommerceContext>(options =>
                 //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            var IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            var connectionString = IsDevelopment ? Configuration.GetConnectionString("DefaultConnection") : GetHerokuConnectionString();

            services.AddDbContext<EcommerceContext>(options => options.UseNpgsql(connectionString));
            // }
            // else
            // {
            //services.AddDbContext<EcommerceContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //}
            services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<EcommerceContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.ClaimsIssuer = Configuration["Authentication:JwtIssuer"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                ValidateIssuer = true,
                ValidIssuer = Configuration["Authentication:JwtIssuer"],

                ValidateAudience = true,
                ValidAudience = Configuration["Authentication:JwtAudience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtKey"])),

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
                };
            });
            services.AddMvc();
            var provider = services.BuildServiceProvider();
            DbContextExtensions.UserManager = provider.GetService<UserManager<AppUser>>();
            DbContextExtensions.RoleManager = provider.GetService<RoleManager<AppRole>>();


                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.ViewLocationExpanders.Add(new FeatureLocationExpander());
                });
        }
        private static string GetHerokuConnectionString()
        {
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};sslmode=Require;Trust Server Certificate=True;";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            //StripeConfiguration.SetApiKey("sk_test_51J03AoJRtbFchhYU4GHtL8lrEXPdDLyGpUb0ScoDOcFktq30O6j1N2HG445IejpV88ZlDfqWxqlxZSu2zNJkh16j00MYm3fGIl");
            StripeConfiguration.ApiKey = "sk_test_51J03AoJRtbFchhYU4GHtL8lrEXPdDLyGpUb0ScoDOcFktq30O6j1N2HG445IejpV88ZlDfqWxqlxZSu2zNJkh16j00MYm3fGIl";
        }
    }
}
