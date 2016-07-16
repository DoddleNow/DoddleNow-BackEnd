using DoddleNow.API.App_Start;
using DoddleNow.API.Infrastructure;
using DoddleNow.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

namespace DoddleNow.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //HttpConfiguration config = new HttpConfiguration();

            //ConfigureOAuth(app);

            //WebApiConfig.AddUser(config);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ////app.UseCors(new CorsOptions
            ////{
            ////    PolicyProvider = new CorsPolicyProvider
            ////    {
            ////        PolicyResolver = context => Task.FromResult(new CorsPolicy
            ////        {
            ////            AllowAnyHeader = true,
            ////            AllowAnyMethod = true,
            ////            AllowAnyOrigin = true,
            ////            SupportsCredentials = false,
            ////            PreflightMaxAge = Int32.MaxValue // << ---- THIS
            ////        })
            ////    }
            ////});

            // app.UseWebApi(config);
            //SecurityConfig.Configure(app);
            // This must happen FIRST otherwise CORS will not work.
            app.UseCors(CorsOptions.AllowAll);

            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            // webapi is registered in the global.asax
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

        }

        
    }
}
