using Shared;
using UsedByExternalLibrary;

namespace LibraryToLoad
{
    public class Implementation : IShared
    {
        public string Name => _dependency.ProvideName;

        private readonly SomeDependency _dependency;

        public Implementation()
        {
            _dependency = new SomeDependency();
        }
    }
}