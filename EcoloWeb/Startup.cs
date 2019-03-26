using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoloWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EcoloWeb.Data.Entity.Identity;

namespace EcoloWeb
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                ;


            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                //Lockout Settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                //User Settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areaRoute",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(services).Wait();
        }

        #region User Role Seed Data    
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var ADMIN_MAIL = "admin@ecoloweb.com";
            var AUDITOR_MAIL = "auditor@ecoloweb.com";
            var DEMO_USER_MAIL = "demo@ecoloweb.com";


            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoles(serviceProvider);


            ApplicationUser AdminUser = new ApplicationUser()
            {
                Name = "Admin",
                Lastname = "User",
                Email = ADMIN_MAIL,
                UserName = ADMIN_MAIL

            };
            await UserManager.CreateAsync(AdminUser, "admin123");


            ApplicationUser AuditorUser = new ApplicationUser()
            {
                Name = "Auditor",
                Lastname = "User",
                Email = AUDITOR_MAIL,
                UserName = AUDITOR_MAIL

            };
            await UserManager.CreateAsync(AuditorUser, "auditor123");



            ApplicationUser Demouser = new ApplicationUser()
            {
                Name = "Demo",
                Lastname = "User",
                Email = DEMO_USER_MAIL,
                UserName = DEMO_USER_MAIL

            };
            await UserManager.CreateAsync(Demouser, "demo123");


            //Asigna Rol a los Usuarios.
            await AddRoleToUser(UserManager, ADMIN_MAIL, "Admin");
            await AddRoleToUser(UserManager, AUDITOR_MAIL, "Auditor");
            await AddRoleToUser(UserManager, DEMO_USER_MAIL, "Usuario");

        }

        private static async Task AddRoleToUser(UserManager<ApplicationUser> UserManager, string userName, string Rol)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(userName);

            await UserManager.AddToRoleAsync(user, Rol);

        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            IdentityResult roleResult;


            //Adding Admin, Usuario Role
            var AdminRoleCheck = await RoleManager.RoleExistsAsync("Admin");
            var AuditorRoleCheck = await RoleManager.RoleExistsAsync("Auditor");
            var UsuarioRoleCheck = await RoleManager.RoleExistsAsync("Usuario");


            //create the roles and seed them to the database
            if (!AdminRoleCheck) { roleResult = await RoleManager.CreateAsync(new ApplicationRole("Admin")); }
            if (!AuditorRoleCheck) { roleResult = await RoleManager.CreateAsync(new ApplicationRole("Auditor")); }
            if (!UsuarioRoleCheck) { roleResult = await RoleManager.CreateAsync(new ApplicationRole("Usuario")); }

        }

        #endregion
    }
}


