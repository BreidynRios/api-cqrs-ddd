using Application.Commons.Settings;
using Application.Commons.Utils;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Middlewares;
using WebApi.Middlewares.Security;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPresentationLayer(this IServiceCollection services,
            IConfiguration configuration, string policyName)
        {
            services.ConfigureHttpClient();
            services.AddCors(configuration, policyName);
            services.AddSecurityBearerAndApiKey(configuration);
            services.AddSwagger();
            services.AddLoggerSeriLog(configuration);
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        private static void AddLoggerSeriLog(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Elasticsearch([new Uri(configuration["ServicesClients:ElasticSearchServices:Host"]!)])
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(logger, dispose: true);
            });
        }

        public static IHost MigrateDatabase<T>(this IHost host, IConfiguration configuration) where T : DbContext
        {
            if (!Convert.ToBoolean(configuration["ApplyMigration"]))
                return host;
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Start migrating for docker");
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return host;
        }

        private static void AddCors(this IServiceCollection services,
            IConfiguration configuration, string policyName)
        {
            var authorizedCorsOrigins = configuration.GetSection("Security:Cors:AuthorizedOrigins")
                .Get<List<string>>();

            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    policy =>
                    {
                        policy.WithOrigins([.. authorizedCorsOrigins])
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });
        }

        private static void ConfigureHttpClient(this IServiceCollection services)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 200
            };
            var sharedClient = new HttpClient(socketsHandler);
            services.AddSingleton(sharedClient);
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api CQRS DDD", Version = "v1" });
                c.AddSecurityDefinition("X-Api-Key", new OpenApiSecurityScheme
                {
                    Name = "X-Api-Key",
                    Description = "ApiKey must appear in header",
                    Type = SecuritySchemeType.ApiKey,                    
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var apiKey = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "X-Api-Key"
                    },
                    In = ParameterLocation.Header
                };
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { apiKey, new List<string>() }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Add valid token",                    
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"                    
                });
                var bearer = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { bearer, new List<string>() }
                });
            });
        }

        private static void AddSecurityBearerAndApiKey(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SecuritySettings>(configuration.GetSection("Security"));

            var jwtConfig = configuration.GetSection("Security:JwtConfig").Get<JwtConfig>()!;
            services.AddSingleton(new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                TokenDecryptionKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(jwtConfig.EncryptionKey)),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(jwtConfig.Key)),
                ClockSkew = TimeSpan.Zero
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = services.BuildServiceProvider()
                        .GetRequiredService<TokenValidationParameters>();
                 })
                .AddScheme<AuthenticationSchemeOptions, BearerTokenAuthenticationHandler>(
                    GeneralConstants.DEFAULT_SCHEME_BEARER_TOKEN, _ => { })
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    GeneralConstants.DEFAULT_SCHEME_API_KEY, _ => { });
        }
    }
}
