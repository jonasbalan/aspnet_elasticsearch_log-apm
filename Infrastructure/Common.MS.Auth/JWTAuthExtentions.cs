using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.MS.Auth
{
    public static class JWTAuthExtentions
    {
        public static IServiceCollection AddJwtSecurity(
      this IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;            

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.MetadataAddress = "http://keycloak:8080/realms/sample-ms-jfb/.well-known/openid-configuration";
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.Validate();
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = x =>
                    {
                        var logger = x.HttpContext.RequestServices.GetService<ILogger<JwtBearerEvents>>();
                        logger.LogError(x.Exception, $"JWT Validation fail - {x.Exception.Message}");
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters.RequireAudience = true;
                options.TokenValidationParameters.ValidAudiences = new string[] { "account" };
                options.TokenValidationParameters.ValidIssuers = new string[] { "https://identity.sample-ms.server.com/realms/sample-ms-jfb" };
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }


        public static async Task<T?> GetJsonWithAuthorizationAsync<T>(this HttpClient client, HttpContext httpContext,  string url)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);            
            var ancestorAuthorization =  httpContext.Request.Headers.Authorization.FirstOrDefault();
            httpRequestMessage.Headers.Add("Authorization", ancestorAuthorization); 
            var response = await client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<T>(content);
            }
            return default(T);
        }
    }
}