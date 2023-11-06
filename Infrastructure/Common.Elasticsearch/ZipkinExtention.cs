using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Steeltoe.Management.Exporter.Tracing;
using Steeltoe.Management.Tracing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Elasticsearch
{
    public static class ZipkinExtention
    {

        public static void ConfigureZipkin(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            //if (!string.IsNullOrEmpty(configuration["ElasticConfiguration:ElasticApm:ServerUrls"]))
            //services.AddDistributedTracing();
            //services.AddZipkinExporter(configuration);
        }

        public static void UseZipkin(this IApplicationBuilder app, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            //app.UseZipkin(configuration);
        }
    }
}
