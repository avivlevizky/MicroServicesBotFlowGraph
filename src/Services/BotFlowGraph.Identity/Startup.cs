using BotFlowGraph.Identity.Data;
using BotFlowGraph.Identity.Models;
using Contracts.Interfaces;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDataAccess;
using System;
using System.Security.Cryptography.X509Certificates;

namespace BotFlowGraph.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            ServiceProvider = serviceProvider;

        }

        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            //var moviesConfig = Configuration.GetSection("AuthMessageSenderOptions")
            //.Get<AuthMessageSenderOptions>();
            services.AddLogging();

            services.AddOptions();

            services.Configure<AuthMessageSenderOptions>(config =>
            {
                Configuration.GetSection("AuthMessageSenderOptions").Bind(config);

            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<IBotMongoDal, BotMongoDal>(_ => new BotMongoDal(_.GetService<ILogger<BotMongoDal>>(), _.GetService<IConfiguration>()));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var keyFilePath = Configuration.GetSection("SigninKeyCredentials").GetValue<string>("KeyFilePath");
            var keyFilePassword = Configuration.GetSection("SigninKeyCredentials").GetValue<string>("KeyFilePassword");
            var cert = new X509Certificate2(keyFilePath, keyFilePassword);

            services.AddIdentityServer(x => x.IssuerUri = @"https://localhost:44329")
                //.AddDeveloperSigningCredential()
                .AddSigningCredential(cert)
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddAspNetIdentity<ApplicationUser>();

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
            }
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();

        }
    }
}
