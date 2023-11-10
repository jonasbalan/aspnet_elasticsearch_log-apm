using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Polly;
using SamplesApi3.Infrastructure;
using System.Text.Json.Serialization;
using Common.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

static void ConfigureSqlOptions(NpgsqlDbContextOptionsBuilder npgsqlOptions)
{
    npgsqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);

    // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 

    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), null);
};

builder.Services.AddDbContext<DoctorScheduleDbContext>(options =>
{
    options.UseNpgsql(DoctorScheduleDbContext.connectionString, ConfigureSqlOptions);
});


builder.Services.AddDbContext<DoctorScheduleDbContext>();

builder.Services.AddHealthChecks();


var app = builder.Build();

app.MapHealthChecks("/healthz");

app.UseAPM(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DoctorScheduleDbContext>();
    var logger = app.Services.GetService<ILogger<DoctorScheduleDbContextSeed>>();    
    await context.Database.MigrateAsync();

    await new DoctorScheduleDbContextSeed().SeedAsync(context, app.Environment, logger);
}

app.Run();
