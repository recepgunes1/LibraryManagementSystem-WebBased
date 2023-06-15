using LMS.Data.Extensions;
using LMS.Service.Extensions;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 4_000
    })
    .AddRazorRuntimeCompilation();
builder.Services.LoadDataLayer(builder.Configuration);
builder.Services.LoadServiceLayer(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller}/{action}/{id?}");
app.MapDefaultControllerRoute();

app.UseNToastNotify();
app.MigrateDatabase();
app.Run();