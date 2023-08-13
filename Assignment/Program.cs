using Assignment.Models;
using Assignment.ServiceContracts;
using Assignment.ServiceImplementations;

namespace Assignment
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.AddControllersWithViews();
      builder.Services.AddHttpClient();
      builder.Services.AddScoped<IFinnhubService, FinnhubService>();
      var app = builder.Build();

      app.UseStaticFiles();
      app.UseRouting();
      app.MapControllers();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      });

      app.Run();
    }
  }
}