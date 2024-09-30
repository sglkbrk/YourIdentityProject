using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development environment
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        //c.RoutePrefix = string.Empty; // Swagger'ı root URL üzerinden açmak için
    });
}

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
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();

//app.UseAuthentication(); 
//app.UseAuthorization();

app.MapControllers();

app.Run();
