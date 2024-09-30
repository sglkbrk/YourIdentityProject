using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;
var builder = WebApplication.CreateBuilder(args);
// IConfiguration nesnesi otomatik olarak yapılandırılır
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 40))));


// Controller'ları ekleyin
builder.Services.AddControllers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

// Desteklenen dilleri tanımlayın
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("tr"),
};

// Yerelleştirme ayarlarını uygulayın
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});


// Middleware'ları ayarlayın
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // Authentication middleware'ı ekleyin
app.UseAuthorization();

app.MapControllers();

app.Run();
