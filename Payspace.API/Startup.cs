using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Payspace.API.Models;
using Payspace.API.Respositories;
using Payspace.Calculations;
using Payspace.Domain.Configuration;
using Payspace.Domain.Interface;

namespace Payspace.API
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=payspace.db");
            });

            services.Configure<JWTOptions>(Configuration.GetSection("JWTOptions"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<ITaxTypeToPostalCodeMapper>(c => new PostCodeToTaxTypeProvider(c.GetService<AppDbContext>()));

            //get the configured Appsettings
            services.AddScoped<IFlatRateCalculator>(c => new FlatRateCalculator(c.GetService<IOptions<AppSettings>>().Value.FlatRateRate));
            services.AddScoped<IFlatValueCalculator>(c => new FlatValueCalculator(c.GetService<IOptions<AppSettings>>().Value.FlatValue, 
                                                    c.GetService<IOptions<AppSettings>>().Value.FlatValueTrigger, 
                                                    c.GetService<IOptions<AppSettings>>().Value.FlatValuePercentage));
            
            services.AddScoped<ITaxCalculationEventLogger>(c =>
                new TaxCalculationEventLogger(c.GetService<AppDbContext>()));
           
            services.AddScoped<IProgressiveTaxRateDataProvider>(c =>

            new ProgressiveTaxRateDataProvider(c.GetService<AppDbContext>()));

            services.AddScoped<IProgressiveRateCalculator>(c => new ProgressiveRateCalculator(c.GetService<IProgressiveTaxRateDataProvider>()));

            
            services.AddScoped(c => new TaxCalculatorManager(
                c.GetService<IProgressiveRateCalculator>(),
                c.GetService<IFlatValueCalculator>(),
                c.GetService<IFlatRateCalculator>()                
                ));
            
            services.AddControllers();
            services.AddSwaggerGen(c => { 
            c.SwaggerDoc("v1", new OpenApiInfo{
                Title = "Payspace API",
                Contact =new OpenApiContact
                {
                    Name="@plasticruler",
                    Email = string.Empty,
                    Url = new Uri("https://twitter.com/precisionv8"),
                },
                Version="v1"});
            });

            //add jwt
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JWTOptions:Key"]))
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            AppDbContext dbContext)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payspace API v1");
                c.RoutePrefix = string.Empty;
            });
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
                dbContext.Database.EnsureCreated();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
