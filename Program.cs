using Microsoft.EntityFrameworkCore;
using data;
using Repository;
using mappings;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Test Attempts API",
        Version = "v1",
        Description = "API для управления попытками прохождения тестов"
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlServerOptions => sqlServerOptions.CommandTimeout(60)));

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IAttemptRepository, AttemptRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Attempts API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        await SeedData.InitializeAsync(context);

        Console.WriteLine("База данных успешно настроена.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при настройке базы данных");
    }
}

app.Run();

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        if (!context.Students.Any())
        {
            var students = new[]
            {
                new Models.Student { Name = "Иван Иванов", Email = "ivan@example.com" },
                new Models.Student { Name = "Петр Петров", Email = "petr@example.com" }
            };
            await context.Students.AddRangeAsync(students);

            var test = new Models.Test
            {
                Title = "Основы C#",
                Description = "Тест по основам программирования на C#",
                IsPublished = true,
                MaxAttempts = 3,
                MaxScore = 100
            };
            await context.Tests.AddAsync(test);

            await context.SaveChangesAsync();
        }
    }
}
