using Application.Services;
using Domain.Entities;
using Domain.Entities.Shared;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // custom services
            builder.Services.AddDbContext<D2DContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Test"));
            });

            builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<D2DContext>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddMediatR(cfg =>
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                cfg.RegisterServicesFromAssemblies(assemblies);
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
