using Common.Elasticsearch;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog(ElasticSearchExtension.ConfigureLogger);

builder.Services.AddHealthChecks();
builder.Services.ConfigureZipkin(builder.Configuration);

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.UseAPM(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseZipkin(app.Configuration);

app.Run();
