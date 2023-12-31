using Common.Elasticsearch;
using SampleApi;
using Serilog;
using Common.MS.Auth;
using Microsoft.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme"
        
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"                                  
                              },
                              In = ParameterLocation.Header,
                              
                          },
                         new string[] {}
                    }
                });
}


    );
builder.Services.AddHttpContextAccessor().AddHttpClient("internalApis").ConfigurePrimaryHttpMessageHandler(x=> {
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
