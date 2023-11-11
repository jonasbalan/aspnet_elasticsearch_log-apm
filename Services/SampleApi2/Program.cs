using Common.Elasticsearch;
using Common.MS.Auth;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor().AddHttpClient("internalApis").ConfigurePrimaryHttpMessageHandler(x => {
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
    return handler;
});
builder.Services.AddJwtSecurity();

builder.Host.UseSerilog(ElasticSearchExtension.ConfigureLogger);

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

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
