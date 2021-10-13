// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Http;
using System.Text;
using VacationHireInc.DataLayer;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.Security;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.ExternalSourceProviders;
using VacationHireInc.WebApi.Interfaces;

namespace VacationHireInc.WebApi
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
            byte[] jwtKey = Encoding.Unicode.GetBytes(Configuration["Jwt:Key"]);
            uint jwtExpirationSeconds = uint.Parse(Configuration["Jwt:ExpirationHours"]) * 60 * 60;
            byte[] passwordSalt = Encoding.Unicode.GetBytes(Configuration["PasswordSalt"]);
            uint currencyApiCacheLifeTimeSec = uint.Parse(Configuration["CurrencyApi:CacheLifeTimeSec"]);
            
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["DbConnectionString"]));
            services.AddSingleton(typeof(IJwtHelper), new JwtHelper(jwtKey, Configuration["Jwt:Issuer"], jwtExpirationSeconds));
            services.AddSingleton(typeof(IHashingHelper), new HashingHelper(passwordSalt));
            services.AddSingleton(typeof(ICurrencyRatesUsdProvider), new CurrencyRatesUsdProvider(Configuration["CurrencyApi:Token"], currencyApiCacheLifeTimeSec, new HttpClient()));
            services.AddScoped<IDataAccessProvider, DataAccessProvider>();
            services.AddControllers();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
                };
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Vacation Hire Inc. API",
                    Description = "API to access Vacation Hire Inc. services.",
                });
                
                // Bearer token authentication
                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                c.AddSecurityDefinition("Bearer", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };
                c.AddSecurityRequirement(securityRequirements);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vacation Hire Inc");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
