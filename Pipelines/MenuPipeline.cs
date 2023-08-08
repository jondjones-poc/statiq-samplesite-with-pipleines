using JonDJones.DotNet.StaticSite.ViewModels;
using Statiq.Markdown;
using Statiq.Yaml;

namespace JonDJones.DotNet.StaticSite.Pipelines;

public class MenuPipeline : Pipeline 
{
    public MenuPipeline() {

        InputModules = new ModuleList
        {
            new ReadFiles(pattern: "pages/*.md"),
        };

        ProcessModules = new ModuleList
        {
            new ExtractFrontMatter(
                new ParseYaml()
            ),
            new RenderMarkdown().UseExtensions(),
            new SetDestination(Config.FromDocument((doc, ctx) =>
            {
                return new NormalizedPath($"menu/{doc.Source.FileNameWithoutExtension}.html");
            }))
        };

        OutputModules = new ModuleList {
            new WriteFiles()
        };
    }
}