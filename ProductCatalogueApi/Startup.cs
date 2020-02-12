using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Facade;
using ProductCatalogue.Repository;
using ProductCatalogue.Repository.EfModel;

namespace ProductCatalogueApi
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
            services.AddMvc();
            services.Configure<AppConfigs>(Configuration.GetSection("AppConfigs"));
            services.AddDbContext<DbContext>(options =>
        options.UseSqlServer(Configuration.GetSection("SqlConnectionString").Value));
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IAggregateRoute, AddProduct>();
            services.AddSingleton<IAggregateRoute, DeleteProduct>();
            services.AddSingleton<IAggregateRoute, UpdateProduct>();
            services.AddSingleton<IAggregateRoute, SearchProduct>();
            services.AddSingleton<IAddProduct, AddProduct>();
            services.AddSingleton<IDeleteProduct, DeleteProduct>();
            services.AddSingleton<IUpdateProduct, UpdateProduct>();
            services.AddSingleton<ISearchProduct, SearchProduct>();
            services.AddSingleton<IProductCatalogueServiceFacade, ProductCatalogueServiceFacade>();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
