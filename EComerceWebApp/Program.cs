using EComerceWebApp;
using EComerceWebApp.Components;
using EComerceWebApp.Components.Pages;

using Microsoft.Extensions.FileProviders;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



builder.Services.AddScoped<ApiService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable static file serving for the default wwwroot folder
app.UseStaticFiles(); // This is for the default wwwroot directory

// Serve static files from c:\Items
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"c:\Items"),
    RequestPath = "/Items" // This will be the URL prefix to access the images
});


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
