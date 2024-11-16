using ViewClient.Repositories.IRepository;
using ViewClient.Repositories.Repository;

namespace ViewClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(6);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<ILogin, Login>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7130");
            });
            builder.Services.AddTransient<ILogin, Login>();
            builder.Services.AddTransient<IRegister, Register>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "login",
                pattern: "login",
                defaults: new { controller = "Authoration", action = "Login" });
            app.MapControllerRoute(
                 name: "register",
                 pattern: "register",
                 defaults: new { controller = "Authoration", action = "Register" });


            app.Run();
        }
    }
}
