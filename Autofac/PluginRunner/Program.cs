using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;
using Autofac.PluginInterfaces;
using Autofac.Configuration;

namespace Autofac.PluginRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            GetPlugins();
            Console.WriteLine(Plugin.Name);
            Console.ReadLine();
        }

        private void GetPlugins()
        {
            var dllDir = new DirectoryInfo(Environment.CurrentDirectory);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("plugins.json")
                .Build();

            var configModule = new ConfigurationModule(configuration);
            var builder = new ContainerBuilder();
            builder.RegisterModule(configModule);
            var container = builder.Build();
                

            var dlls = dllDir.GetFiles("*.dll")
                .Select(f => AssemblyLoadContext.Default.LoadFromAssemblyPath(f.FullName))
                .ToList();

            try
            {
                // Always resolve from a scope.
                // https://autofac.readthedocs.io/en/latest/best-practices/index.html#always-resolve-dependencies-from-nested-lifetimes
                using (var scope = container.BeginLifetimeScope())
                {
                    var plugin = scope.Resolve<INamedPlugin>();
                    Console.WriteLine("Resolved specific plugin type: {0}", plugin.Name);

                    Console.WriteLine("All available plugins:");
                    var allPlugins = scope.Resolve<IEnumerable<INamedPlugin>>();
                    foreach (var resolved in allPlugins)
                    {
                        Console.WriteLine("- {0}", resolved.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error during configuration demonstration: {0}", ex);
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        public INamedPlugin Plugin { get; private set; }
    }
}
