using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diplom.api.DataAccessLayer;
using diplom.api.DataAccessLayer.Implementation;
using diplom.api.Providers;
using diplom.api.Providers.Implementation;
using diplom.api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace diplom.api
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

            services.AddMemoryCache();
            services.AddCors();

            services.AddSingleton(Configuration.GetSection("DataAccessSettings").Get<DataAccessSettings>());
            services.AddSingleton(typeof(IDataAccessAdapter),typeof(DataAccessAdapter));
            services.AddSingleton(typeof(IPaintingProvider), typeof(PaintingProvider));
            services.AddSingleton(typeof(IUserProvider), typeof(UserProvider));
            services.AddSingleton(typeof(IGenreProvider), typeof(GenreProvider));
            services.AddSingleton(typeof(IAlbumProvider), typeof(AlbumProvider));
            services.AddSingleton(typeof(IAuctionProvider), typeof(AuctionProvider));
            services.AddSingleton(typeof(IOrderProvider), typeof(OrderProvider));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
