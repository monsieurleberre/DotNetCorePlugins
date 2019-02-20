using System.Composition;
using Mef.PluginInterfaces;

namespace Mef.Plugin2
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
