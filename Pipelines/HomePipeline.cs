using JonDJones.DotNet.StaticSite.ViewModels;
using Statiq.Razor;
using Statiq.Yaml;

namespace JonDJones.DotNet.StaticSite.Pipelines;

public class HomePipeline : LayoutPipeline {
    public HomePipeline() {

        InputModules = new ModuleList
        {
            new ReadFiles("pages/home.md")
        };

        ProcessModules = new ModuleList {
            new ExtractFrontMatter(
                new ParseYaml()
            ),
            new MergeContent(new ReadFiles(patterns: "Home.cshtml")),
            new RenderRazor().WithModel(Config.FromDocument(async (document, context) =>
            {
                var title = document.GetString("Title");
                var content = await context.FileSystem.GetInputFile(document.Source).ReadAllTextAsync();

                return new HomeViewModel
                {
                    Title = title,
                    Content = content.Split("---")[1]
                };
            })),
            new SetDestination(Config.FromDocument((doc, ctx) => {
                return new NormalizedPath("index.html");
            }))
        };
        OutputModules = new ModuleList {
            new WriteFiles()
        };
    }
}
