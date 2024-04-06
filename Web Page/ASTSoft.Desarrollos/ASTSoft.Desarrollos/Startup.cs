using Infrastructure.Model;
using IRepositories;
using IServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.DomainModels;
using Repositories;
using Services;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASTSoft.Desarrollos
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
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
            services.AddDbContext<ASTDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddControllersWithViews();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(ITimeZoneService), typeof(TimeZoneService));
            ConfigurarBaseDeDatos(services);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
            });

            services.AddDistributedMemoryCache();
            services.AddControllersWithViews();

            services.AddMvc().AddRazorRuntimeCompilation();

            //services.AddScoped(typeof(IOpcionesMenuRepository), typeof(OpcionesMenuRepository));

            //Servicios
            services.AddScoped(typeof(IReciboMercaderiaService), typeof(ReciboMercaderiaService));
            services.AddScoped(typeof(IAST_UsuariosService), typeof(AST_UsuariosService));
            services.AddScoped(typeof(IHistorialServices), typeof(HistorialServices));
            services.AddScoped(typeof(IProcesosServices), typeof(ProcesosServices));
            services.AddScoped(typeof(IAST_RutasServices), typeof(AST_RutasServices));
            services.AddScoped(typeof(IArticulosServices), typeof(ArticulosServices));
            services.AddScoped(typeof(IEtiquetaServices), typeof(EtiquetaServices));
            services.AddScoped(typeof(ICanastaService), typeof(CanastaServices));
            services.AddScoped(typeof(IPredespachoServices), typeof(PredespachoServices));
            services.AddScoped(typeof(IDatamatrixServices), typeof(DatamatrixServices));
            services.AddScoped(typeof(IConsultaServices), typeof(ConsultaServices));
            services.AddScoped(typeof(IAlmService), typeof(AlmService));
            services.AddScoped(typeof(IAST_DocumentosEmpaqueServices), typeof(AST_DocumentoEmpaqueServices));

            services.Configure<IdentityOptions>(options =>
            {
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddMvc();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(4320);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });
            services.AddHttpContextAccessor();
            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
            Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../PDF");
        }

        public void ConfigurarBaseDeDatos(IServiceCollection services)
        {
            var connectionString = new Services.ConnectionString(Configuration.GetConnectionString("defaultConnection"));
            services.AddSingleton(connectionString);
        }
    }
}
