using System;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;
using Autofac.PluginInterfaces;

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

            var authorisedPlugin = "Plugin2";
                

            var dlls = dllDir.GetFiles("*.dll")
                .Where(f => (authorisedPlugin + ".dll").Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))
                .Select(f => AssemblyLoadContext.Default.LoadFromAssemblyPath(f.FullName))
                .ToList();

            var containerConfig = new ContainerConfiguration().WithAssemblies(dlls);
            using (var container = containerConfig.CreateContainer())
            {
                Plugin = container.GetExport<INamedPlugin>();
            }
        }

        public INamedPlugin Plugin { get; private set; }
    }
}
