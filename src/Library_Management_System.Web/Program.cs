using Library_Management_System.Data.Extensions;
using Library_Management_System.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.LoadDataLayer(builder.Configuration);
builder.Services.LoadServiceLayer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller}/{action}/{id?}");

app.Run();