using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Shared;

namespace AssemblyLoader
{
    class Program
    {
        public static int Main(string[] args)
        {

            try
            {
                var pathToDlls = Path.Combine(Environment.CurrentDirectory, "..", "LibraryToLoad");
                var filesToLoad = Directory.EnumerateFiles(pathToDlls, "*.dll", SearchOption.AllDirectories);
                var localImplementation = new LocalImplementation(); 
            
                var assemblies = filesToLoad.Select(
                    dll =>
                    {
                        Console.WriteLine($"Loading {dll}");
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                        PrintTypes(assembly);
                        return assembly;
                    }).ToList();
                
                var type = assemblies.SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == "Implementation");
                
                if(type == null) return 1;
                    
                var implementation = Activator.CreateInstance(type);
                var name = ((IShared) implementation).Name;
                    
                Console.WriteLine(name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return 0;
        }

        private static void PrintTypes(Assembly assembly)
        {
            foreach (TypeInfo type in assembly.DefinedTypes)
            {
                Console.WriteLine(type.Name);
                foreach (PropertyInfo property in type.DeclaredProperties)
                {
                    string attributes = string.Join(
                        ", ",
                        property.CustomAttributes.Select(a => a.AttributeType.Name));

                    if (!string.IsNullOrEmpty(attributes))
                    {
                        Console.WriteLine("    [{0}]", attributes);
                    }
                    Console.WriteLine("    {0} {1}", property.PropertyType.Name, property.Name);
                }
            }
        }

        public class LocalImplementation : IShared
        {
            public string Name => "LocalImplementation";
        }
    }
}