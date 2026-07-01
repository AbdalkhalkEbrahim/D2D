using Application.Hubs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Shared;
using Domain.Settings;
using Hangfire;
using Hangfire.MemoryStorage;
using Infrastructure.Background_services;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // custom services
            string openAI_APIKey = builder.Configuration["OpenAI:ApiKey"];
            string dbConn = builder.Configuration.GetConnectionString("Test");

            #region Swagger Settings
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 8 Web API",
                    Description = " ITI Projrcy"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                    }
                });
            });
            #endregion
            builder.Services.AddDbContextPool<D2DContext>(options =>
            {
                options.UseSqlServer(dbConn);
            });

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
            });

            /*            builder.Services.AddHostedService<OtpCleanupBackgroundWorker>();
                        builder.Services.AddHostedService<RefreshTokenCleanupBackgroundWorker>();
                        builder.Services.AddHostedService<MagicTokenCleaupBackgroundWorker>();*/
            builder.Services.AddHostedService<D2DBackgroundServices>();

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUploadService, UploadService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<IDesignValidationService,DesignValidationService>();
            builder.Services.AddScoped<IIdentityValidationService>(provider=>new IdentityValidationService(openAI_APIKey));
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.AllowedForNewUsers = true;   
                options.Lockout.MaxFailedAccessAttempts = 4; 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
             .AddEntityFrameworkStores<D2DContext>()
             .AddDefaultTokenProviders();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("jwt"));
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Bind your JWT options
                var jwtSettings = builder.Configuration.GetSection("jwt").Get<JwtSettings>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,       // Adjust properties based on your JwtSettings class
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"]; 
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            builder.Services.AddMediatR(cfg =>
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                cfg.RegisterServicesFromAssemblies(assemblies);
            });
            builder.Services.AddSignalR();
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("https://design-to-dress.vercel.app")
                     .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHangfire(config => config
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseMemoryStorage());

            builder.Services.AddHangfireServer();

            var app = builder.Build();
            //app.UseHangfireDashboard("/hangfire");
            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "D2D API V1");
                c.RoutePrefix = "swagger";
            }
             );

                app.MapSwagger();
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();

            app.UseAuthorization();
            app.MapHub<NotificationHub>("/notificationhub");
            app.MapHub<ChatHub>("/chathub");
            app.UseHangfireDashboard("/hangfire");

            app.MapControllers();


            app.Run();
        }
    }
}
