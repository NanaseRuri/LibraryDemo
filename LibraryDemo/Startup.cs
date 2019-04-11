using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LibraryDemo.CustomConvention;
using LibraryDemo.CustomPolicy;
using LibraryDemo.Data;
using LibraryDemo.Infrastructure;
using LibraryDemo.Middleware;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace LibraryDemo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<LendingInfoDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LendingInfoDbContext"));
            });
            services.AddDbContext<StudentIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("StudentIdentityDbContext"));
            });
            services.AddIdentity<Student, IdentityRole>(opts =>
                {
                    opts.Lockout.MaxFailedAccessAttempts = 3;
                    opts.User.RequireUniqueEmail = true;
                    opts.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;                    
                }).AddEntityFrameworkStores<StudentIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("CompletedProbation",
                    policy => policy.Requirements.Add(new EmploymentDurationRequirement(3)));
            });
            services.AddSingleton<IAuthorizationHandler, MinimumEmploymentDurationHandler>();
            services.AddCors(opts =>
            {
                opts.AddPolicy("ApiPolicy", builder => builder.WithOrigins("https://fuck.com"));
            });
            services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.HttpOnly = true;
                opts.LoginPath = "/StudentAccount/Login";
                opts.AccessDeniedPath = "/StudentAccount/Login";
                opts.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });            
            services.AddSingleton<EmailSender>();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Never",new CacheProfile()
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
                options.CacheProfiles.Add("Normal",new CacheProfile()
                {
                    Location = ResponseCacheLocation.Client,
                    Duration = 90
                });
                options.Conventions.Add(new AutoValidationAntiForgeryTokenActionModelConvention());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddDistributedMemoryCache();
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = Configuration.GetConnectionString("cache");
            //    options.SchemaName = Configuration.GetValue<string>("cache.schemaName");
            //    options.TableName = Configuration.GetValue<string>("cache.tableName");
            //});
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=BookInfo}/{action=Index}");
            });
            //app.Use(async (content, next) =>
            //{
            //    content.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
            //    await next.Invoke();
            //});
            app.UseMiddleware<CSPMiddleware>();
            app.Map("/BookInfo", appBuilder => { appBuilder.UseMiddleware<CSPMiddleware>(); });
            StudentInitiator.InitialStudents(app.ApplicationServices).Wait();
            BookInitiator.BookInitial(app.ApplicationServices).Wait();
        }
    }
}
