using StructureMap;
using StructureMap.Graph;

namespace FileLoader
{
    public static class Bootstrap
    {
        public static void InitIoC()
        {
            var container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.WithDefaultConventions();
                });
            });

            container.GetInstance<IFileLoader>();
                
        }
    }
}
