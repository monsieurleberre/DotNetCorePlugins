using System;
using Autofac.PluginInterfaces;

namespace Autofac.Plugin1
{
    public class NamedPlugin1 : INamedPlugin
    {
        public string Name { get; }

        public NamedPlugin1()
        {
            Name = "plugin1";
        }
    }
}
