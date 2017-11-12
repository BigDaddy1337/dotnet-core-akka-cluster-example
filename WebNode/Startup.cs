using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebNode.Middlewares;
using WebNode.Services;

namespace WebNode
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(serviceProvider => ActorSystem.Create("simple-actorsystem-app"));
            services.AddSingleton<IActorSystemService, ActorSystemService>();

            services.AddSingleton<IClusterBuilder, ClusterBuilder>();
            services.AddSingleton<IClusterActorSystemService, ClusterActorSystemService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, IClusterBuilder clusterBuilder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseLog();
            app.UseMvc();

            applicationLifetime.ApplicationStarted.Register(clusterBuilder.StartClusterNode);
            applicationLifetime.ApplicationStopped.Register(clusterBuilder.StopClusterNode);
        }
    }
}
