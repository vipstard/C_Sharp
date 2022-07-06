using Hello.BLL;
using Hello.IDAL;
using Hello.Oracle.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hello.MVC6
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

            /*  MVC 6 - 의존성 주입을 할 수 있는 3가지. */
            // 1. AddSingleton<T>();
            // - 웹사이트가 시작하면 사이트가 종료될 때까지 메모리 상에 유지되는 객체
            // 2. .AddScoped<T>();
            // - 웹사이트가 시작되어 1번의 요청이 있을 때 메모리상에 유지되는 객체
            // 3. .AddTrasient<T>();
            // - 웹 사이트가 시작되어 각 요청마다 새롭게 생성되는 객체 주입

            services.AddTransient<UserBLL>();

            // 인터페이스와 실제 구현체를 적어줘야한다.
            services.AddTransient<IUserDal, UserDal>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
