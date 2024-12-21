using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories;
using Microsoft.EntityFrameworkCore;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.ApiAccess;
using Microsoft.OpenApi.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;
using FluentAssertions.Common;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services.JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        //DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Repository + UoW
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IProductionFacilityRepository, ProductionFacilityRepository>();
        // services
        builder.Services.AddScoped<EquipmentPlacementContractService>();
        builder.Services.AddScoped<ProductionFacilityService>();
        builder.Services.AddSingleton<IBackgroundTaskQueue>(_ => new BackgroundTaskQueue(100));
        builder.Services.AddHostedService<QueuedHostedService>();


        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            // Определение API-ключа
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "ApiKey", 
                Type = SecuritySchemeType.ApiKey,
                Description = "Enter your API key here"
            });

            // Требование API-ключа для всех операций
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
              {
                 new OpenApiSecurityScheme
                 {
                  Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                  }
                 },
                Array.Empty<string>()
              }
            });
        });



        var app = builder.Build();

       

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Используем ApiKeyMiddleware для проверки API-ключа
        app.UseMiddleware<ApiKeyMiddleware>();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
