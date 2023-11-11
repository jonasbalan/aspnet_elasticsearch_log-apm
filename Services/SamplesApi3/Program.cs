using Common.Elasticsearch;
using Common.MS.Auth;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using SamplesApi3.Infrastructure;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddDbContext(builder);

builder.Host.UseSerilog(ElasticSearchExtension.ConfigureLogger);


builder.Services.AddHttpClient();
builder.Services.AddJwtSecurity();

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

app.UseAuthentication();

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

static void AddDbContext(WebApplicationBuilder builder)
{
    static void ConfigureSqlOptions(NpgsqlDbContextOptionsBuilder npgsqlOptions)
    {
        npgsqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);

        npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), null);
    };

    builder.Services.AddDbContext<DoctorScheduleDbContext>(options =>
    {
        options.UseNpgsql(DoctorScheduleDbContext.connectionString, ConfigureSqlOptions);
    });
}