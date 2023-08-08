namespace JonDJones.DotNet.StaticSite.Pipelines
{
    public class AssetsPipeline : Pipeline
    {
        public AssetsPipeline()
        {
            Isolated = true;
            ProcessModules = new ModuleList
            {
                new CopyFiles(
                    "./{css,fonts,js,images}/**/*",
                    "./vendor/**/*",
                    "./css/**/*",
                    "*.{png,jpg,ico,webmanifest,cshtml}")
            };
        }
    }
}
