var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Example of how to load the web ui from a module
// var assembly = typeof(ModularMonolithModuleAssemblyInfo).Assembly;
// builder.Services.AddRazorPages().AddApplicationPart(assembly).AddRazorRuntimeCompilation();
// builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
// {
//     options.FileProviders.Add(new EmbeddedFileProvider(assembly));
// });
  
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapGet("/meta/name", () => Assembly.GetExecutingAssembly().GetName());
app.MapGet("/health/alive", () => "alive");
app.MapGet("/health/ready", () => "ready");
app.MapRazorPages();
app.Run();
