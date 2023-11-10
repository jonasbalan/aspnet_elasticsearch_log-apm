using Elastic.Apm.NetCoreAll;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Serilog.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Elastic.Apm.EntityFrameworkCore;

namespace Common.Elasticsearch
{
    public static class ElasticSearchExtension
    {
        public static void UseAPM(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration["ElasticConfiguration:ElasticApm:ServerUrls"]))
                app.UseAllElasticApm(configuration.GetSection("ElasticConfiguration"));
        }

        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (context, loggerConfiguration) =>
        {
            var env = context.HostingEnvironment;
            loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                //.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console();
            //if (context.HostingEnvironment.IsDevelopment())
            //{
            //    loggerConfiguration.MinimumLevel.Override("SampleApi", LogEventLevel.Debug);              
            //}

            var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            if (!string.IsNullOrEmpty(elasticUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = $"jfb-logs-{GetApiName()}-{0:yyyy.MM.dd}",
                        MinimumLogEventLevel = LogEventLevel.Debug,
                        IndexAliases = new string[] { $"jfb-{GetApiName()}" }

                    });
            }
        };

        private static string GetApiName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

    }
}
