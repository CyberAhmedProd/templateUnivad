using ESP.BLL;
using ESP.DAL;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Data Protection : stocker les clés dans le dossier du projet
var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("ESP");

builder.Services.AddControllersWithViews();

// DAL
builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddScoped<SessionDAL>();
builder.Services.AddScoped<SessionSeanceDAL>();
builder.Services.AddScoped<SessionFiliereDAL>();
builder.Services.AddScoped<SessionNiveauDAL>();
builder.Services.AddScoped<SessionJourDAL>();
builder.Services.AddScoped<SessionCentreDAL>();
builder.Services.AddScoped<SessionNiveauCentreDAL>();
builder.Services.AddScoped<SessionEpreuveDAL>();
builder.Services.AddScoped<SessionCreditsDAL>();

// BLL
builder.Services.AddScoped<SessionBLL>();
builder.Services.AddScoped<SessionSeanceBLL>();
builder.Services.AddScoped<SessionFiliereBLL>();
builder.Services.AddScoped<SessionNiveauBLL>();
builder.Services.AddScoped<SessionJourBLL>();
builder.Services.AddScoped<SessionCentreBLL>();
builder.Services.AddScoped<SessionNiveauCentreBLL>();
builder.Services.AddScoped<SessionEpreuveBLL>();
builder.Services.AddScoped<SessionCreditsBLL>();

var app = builder.Build();

// Initialiser la base SQLite
DbInitializer.Initialize(app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Session}/{action=Index}/{id?}");

app.Run();
