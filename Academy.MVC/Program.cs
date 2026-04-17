using Academy.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ApiClient>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl ?? "https://localhost:7200/");
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 🔥 BU MÜTLƏQ OLMALIDIR (css, js işləməsi üçün)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Areas routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Group}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "studentArea",
    pattern: "{area:exists}/{controller=Attendance}/{action=Index}/{id?}"
);

// Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();