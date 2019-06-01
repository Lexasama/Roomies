using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Controllers;
using ITI.Roomies.WebApp.Services;
using ITI.Roomies.WebApp.Services.Email;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace ITI.Roomies.WebApp
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddOptions();

            services.AddMvc();
            services.AddSingleton( _ => new RoomiesGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new CollocGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new CollRoomGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new RoomiesGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new ItemGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new TasksGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new TaskRoomGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton(_ => new CourseGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton(_ => new ImageGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new CategoryGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new BudgetGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );
            services.AddSingleton( _ => new TransactionGateway( Configuration["ConnectionStrings:RoomiesDB"] ) );


            services.AddSingleton<PasswordHasher>();
            services.AddSingleton<RoomiesService>();
            services.AddSingleton<TokenService>();
            services.AddSingleton<GoogleAuthenticationManager>();

            //services.AddSingleton<IEmailConfiguration>( Configuration.GetSection( "EmailConfiguration" ).Get<EmailConfiguration>() );
            services.AddTransient<IEmailService, EmailService>();

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            services.Configure<TokenProviderOptions>(o =>
            {
                o.Audience = Configuration["JwtBearer:Audience"];
                o.Issuer = Configuration["JwtBearer:Issuer"];
                o.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<SpaOptions>( o =>
            {
                o.Host = Configuration[ "Spa:Host" ];
            } );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddAuthentication(CookieAuthentication.AuthenticationScheme)
                .AddCookie(CookieAuthentication.AuthenticationScheme, o =>
               {
                   o.ExpireTimeSpan = TimeSpan.FromHours(1);
                   o.SlidingExpiration = true;
               })
                .AddJwtBearer(JwtBearerAuthentication.AuthenticationScheme, o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = signingKey,

                       ValidateIssuer = true,
                       ValidIssuer = Configuration["JwtBearer:Issuer"],

                       ValidateAudience = true,
                       ValidAudience = Configuration["JwtBearer:Audience"],

                       NameClaimType = ClaimTypes.Email,
                       AuthenticationType = JwtBearerAuthentication.AuthenticationType,

                       ValidateLifetime = true
                   };
               })
                .AddGoogle(o =>
               {
                   o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                   o.ClientId = Configuration["Authentication:Google:ClientId"];
                   o.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                   o.Events = new OAuthEvents
                   {
                       OnCreatingTicket = ctx => ctx.HttpContext.RequestServices.GetRequiredService<GoogleAuthenticationManager>().OnCreatingTicket(ctx)
                   };
                   o.AccessType = "offline";
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowCredentials();
                c.WithOrigins(Configuration["Spa:Host"]);
            });

            string secretKey = Configuration["JwtBearer:SigningKey"];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Account", action = "Login" });
            });

            app.UseStaticFiles();
        }

    }
}
