using System.Composition;
using PluginInterfaces;

namespace Plugin2
{
    [Export(typeof(INamedPlugin))]
    public class NamedPlugin2 : INamedPlugin
    {
        public string Name { get; }

        public NamedPlugin2()
        {
            Name = "plugin2";
        }
    }
}
