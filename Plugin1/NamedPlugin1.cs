using System;
using System.Composition;
using PluginInterfaces;

namespace Plugin1
{
    [Export(typeof(INamedPlugin))]
    public class NamedPlugin1 : INamedPlugin
    {
        public string Name { get; }

        public NamedPlugin1()
        {
            Name = "plugin1";
        }
    }
}
