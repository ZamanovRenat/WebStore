using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Services;
using WebStore.Services.InCookies;
using WebStore.Services.InMemory;
using WebStore.Services.InMemory.InSQL;
using WebStore.Services.Interfaces;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //Подключение контекста базы данных
            services.AddDbContext<WebStoreDB>(opt => 
                opt.UseSqlServer(Configuration.GetConnectionString("MSSQL")));
            
            //AddTransient удаляет объект после использования
            services.AddTransient<WebStoreDBInitializer>();

            //Подключение идентификации
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            //Конфигурирование Identity
            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequireDigit = false;           //в пароле необязательны цифры
                opt.Password.RequiredLength = 3;             //длина пароля 3 символа
                opt.Password.RequireLowercase = false;       //в пароле необязательны буквы с нижним регистром
                opt.Password.RequireUppercase = false;       //в пароле необязательны буквы с верхним регистром
                opt.Password.RequireNonAlphanumeric = false; //в пароле необязательны алфавитные символы
                opt.Password.RequiredUniqueChars = 3;        //3 уникальных символа
#endif
                opt.User.RequireUniqueEmail = false;    //отключено требование уникальности e-mail
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; //Из каких символов может состоять код
                opt.Lockout.AllowedForNewUsers = false; //не блокировать новых пользователей
                opt.Lockout.MaxFailedAccessAttempts = 10; //max кол-во подключиться
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); //время блокировки при при превышение попыток
            });

            //Конфигурирование Cookies
            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "MaliasStore";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });
            
            //Добавляем сервис управления сотрудниками
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            
            //Добавление сервиса корзины
            services.AddScoped<ICartService, InCookiesCartService>();

            //Добавляем сервисы, необходимые для mvc
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //Добавляем сервис управления брэндами и секциями
            //services.AddSingleton<IProductData, InMemoryProductData>();
            if (Configuration["ProductsDataSource"] == "db")
                services.AddScoped<IProductData, SqlProductData>();
            else
                services.AddSingleton<IProductData, InMemoryProductData>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            //Создание отдельной области через которую инициальзируется БД, после объект уничтожается
            using (var scope = services.CreateScope())
                scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Поддержка статических файлов 
            app.UseStaticFiles();

            app.UseRouting();

            //добавление Identity
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

        }
    }
}
