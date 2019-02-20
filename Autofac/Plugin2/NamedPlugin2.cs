using System.Composition;
using Autofac.PluginInterfaces;

namespace Autofac.Plugin2
{
    public class NamedPlugin2 : INamedPlugin
    {
        public string Name { get; }

        public NamedPlugin2()
        {
            Name = "plugin2";
        }
    }
}
