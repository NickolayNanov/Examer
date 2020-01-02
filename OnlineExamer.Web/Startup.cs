namespace OnlineExamer.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Components.Authorization;

    using OnlineExamer.Infrastructure;
    using OnlineExamer.Core.ExamService;
    using OnlineExamer.Models.Entities;
    using OnlineExamer.Data;
    using OnlineExamer.Web.Areas.Identity.Pages.Account;
    using OnlineExamer.Data.Seeding;
    using OnlineExamer.Core.SchoolSubjects;
    using OnlineExamer.Infrastructure.SendGrid;   
    using OnlineExamer.Core;
    using OnlineExamer.Core.AdminService;

    using AutoMapper;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OnlineExamerDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<OnlineExamerUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<OnlineExamerDbContext>()
            .AddRoles<IdentityRole>();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<OnlineExamerUser>>();
           
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ISchoolSubjectService, SchoolSubjectService>();

            services.AddTransient<SendGrid>();
            services.AddTransient<LogoutModel>();
            services.AddTransient<SendGrid>();
            services.AddTransient<UserRolesSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<OnlineExamerDbContext>();
                Seeder.Seed(dbContext, serviceScope.ServiceProvider);
            }

            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");  
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
