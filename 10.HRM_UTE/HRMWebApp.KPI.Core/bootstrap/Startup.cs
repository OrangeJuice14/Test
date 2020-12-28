using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
#if(authentication=="identity") {
using Microsoft.AspNetCore.Identity;
using DevExpressProjectTemplate.Data;
using DevExpressProjectTemplate.Models;
using DevExpressProjectTemplate.Services;
#endif

namespace DevExpressProjectTemplate {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
#if(authentication =="identity") {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
#endif

            services.AddMvc();
            services.AddDevExpressControls(options => {
                options.Bootstrap(bootstrapOptions => {
                    bootstrapOptions.IconSet = BootstrapIconSet.Embedded;
                    bootstrapOptions.Mode = BootstrapMode.Bootstrap4;
                });
                options.Resources = ResourcesType.DevExtreme;
            });
            // Sample data context registration
            services.AddDbContext<DevExpressProjectTemplate.Models.NorthwindContext>(options => {
                options.UseSqlite(Configuration.GetConnectionString("NorthwindConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseDevExpressControls();
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
#if(authentication =="identity") {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
#endif
            } else
                app.UseExceptionHandler("/Home/Error");
            app.UseStaticFiles();
#if(authentication =="identity") {
            app.UseAuthentication();
#endif
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
