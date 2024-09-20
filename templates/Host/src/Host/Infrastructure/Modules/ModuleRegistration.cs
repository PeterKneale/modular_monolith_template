﻿using Common;

namespace Host.Infrastructure.Modules;

public static class ModuleRegistration
{
    public static void AddModules(this IServiceCollection services)
    {
        Console.WriteLine("Loading Modules");

        var assemblies = GetAssemblyNames().ToList();

        foreach (var type in GetModules(assemblies))
        {
            Console.WriteLine($"Registering Module {type.Assembly}");
            services.AddSingleton(typeof(IModule), type);
        }

        foreach (var type in GetModuleStarts(assemblies))
        {
            Console.WriteLine($"Registering Module Startup {type.Assembly}");
            services.AddSingleton(typeof(IModuleStartup), type);
        }
    }

    public static void StartModules(this WebApplication app)
    {
        Console.WriteLine("Starting Modules...");
        var modules = app.Services.GetRequiredService<IEnumerable<IModuleStartup>>();
        foreach (var module in modules)
        {
            Console.WriteLine($"Starting {module.GetType().Assembly}...");
            module.Startup();
            Console.WriteLine($"Started {module.GetType().Assembly}");
        }
    }

    public static void AddWebModules(this IMvcBuilder mvc)
    {
        var assemblies = GetAssemblyNames();
        foreach (var assembly in GetWebModules(assemblies))
        {
            mvc.AddApplicationPart(assembly);
        }
    }

    public static void AddWebModulesFiles(this IServiceCollection services)
    {
        var assemblies = GetAssemblyNames();
        foreach (var assembly in GetWebModules(assemblies))
        {
            services.Configure<MvcRazorRuntimeCompilationOptions>(options => { options.FileProviders.Add(new EmbeddedFileProvider(assembly)); });
        }
    }

    public static void UseApiModules(this WebApplication app)
    {
        var assemblies = GetAssemblyNames();
        foreach (var method in GetApiModules(assemblies))
        {
            method.Invoke(null, new object[] { app });
        }
    }

    private static IEnumerable<Assembly> GetWebModules(IEnumerable<string> list)
    {
        foreach (var assembly in list)
        {
            if (!assembly.ToLowerInvariant().EndsWith(".web.dll")) continue;
            var webModules = Load(assembly);
            yield return webModules;
        }
    }

    // Get the single type that implements the IModule interface from an assembly
    private static IEnumerable<Type> GetModules(IEnumerable<string> files)
    {
        var list = new List<Type>();
        foreach (var file in files)
        {
            try
            {
                var assembly = Load(file);
                list.AddRange(assembly.GetTypes().Where(x => x.IsClass && x.IsAssignableTo(typeof(IModule)))
                    .ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot load assembly {file}" + e.Message);
            }
        }

        return list;
    }

    // Get the single type that implements the IModule interface from an assembly
    private static IEnumerable<Type> GetModuleStarts(IEnumerable<string> files)
    {
        var list = new List<Type>();
        foreach (var file in files)
        {
            try
            {
                var assembly = Load(file);
                list.AddRange(assembly.GetTypes().Where(x => x.IsClass && x.IsAssignableTo(typeof(IModuleStartup))));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot load {file}" + e.Message);
            }
        }

        return list;
    }

    private static IEnumerable<MethodInfo> GetApiModules(IEnumerable<string> list)
    {
        foreach (var assembly in list)
        {
            if (!assembly.ToLowerInvariant().EndsWith(".api.dll")) continue;
            var module = Load(assembly);
            yield return GetRegistrationMethod(module);
        }
    }


    private static MethodInfo GetRegistrationMethod(Assembly assembly)
    {
        // Find all types in the assembly
        var types = assembly.GetTypes();

        // Look for types with the static extension method RegisterEndpoints(WebApplication app)
        foreach (var type in types)
        {
            var methodInfo = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == "RegisterEndpoints" &&
                                     m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false));

            if (methodInfo != null && methodInfo.GetParameters().Length == 1 && methodInfo.GetParameters()[0].ParameterType == typeof(WebApplication))
            {
                return methodInfo;
            }
        }

        throw new NotImplementedException("Cant find RegistrationMethod");
    }


    private static IEnumerable<string>? _assemblyNames;

    private static IEnumerable<string> GetAssemblyNames()
    {
        if (_assemblyNames != null) return _assemblyNames;
        var ignore = new[] { "microsoft", "system", "xunit" };
        var list = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").ToList();
        _assemblyNames = list
            .Where(item => !ignore.Any(x => item.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
            .ToList();
        return _assemblyNames;
    }

    private static Assembly Load(string s) => Assembly.Load(AssemblyName.GetAssemblyName(s));
}