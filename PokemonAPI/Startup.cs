using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PokemonAPI.Domain.Mappers;
using PokemonAPI.Domain.Providers;
using PokemonAPI.Domain.Services;
using PokemonAPI.Domain.Wrappers;

namespace PokemonAPI
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokemonAPI", Version = "v1" });
            });

            services.AddTransient<IPokemonMapper, PokemonMapper>();
            services.AddTransient<IPokemonService, PokemonService>((ctx) =>
            {
                var client = new HttpClientWrapper(Configuration["PokemonEndpointUrl"]);
                return new PokemonService(client, ctx.GetService<IPokemonMapper>());
            });
            services.AddTransient<ITranslationService, TranslationService>((ctx) =>
            {
                var client = new HttpClientWrapper(Configuration["TranslationEndpointUrl"]);
                return new TranslationService(client, ctx.GetService<ILogger<TranslationService>>());
            });
            services.AddScoped<IPokemonProvider, PokemonProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokemonAPI v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
