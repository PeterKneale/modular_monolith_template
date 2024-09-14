var builder = WebApplication.CreateBuilder(args);

// Example of how to load the web ui from a module
var assembly = typeof(TenancyAssemblyInfo).Assembly;
builder.Services.AddRazorPages().AddApplicationPart(assembly).AddRazorRuntimeCompilation();
builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{
    options.FileProviders.Add(new EmbeddedFileProvider(assembly));
});
  
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();
app.MapGet("/", () => "Hello World!");
app.MapRazorPages();
app.Run();
