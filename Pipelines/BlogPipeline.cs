using JonDJones.DotNet.StaticSite.ViewModels;
using Statiq.Razor;
using Statiq.Yaml;

namespace JonDJones.DotNet.StaticSite.Pipelines;

public class BlogPipeline : LayoutPipeline {
    public BlogPipeline() {

        InputModules = new ModuleList
        {
            new ReadFiles(pattern: "posts/*.md"),
        };

        ProcessModules = new ModuleList {
            new ExtractFrontMatter(
                new ParseYaml()
            ),
            new MergeContent(new ReadFiles(patterns: "Blog.cshtml")),
            new RenderRazor().WithModel(Config.FromDocument(async (document, context) =>
            {
                var title = document.GetString("Title");
                var content = await context.FileSystem.GetInputFile(document.Source).ReadAllTextAsync();
                var date =  document.GetDateTime("Published").ToLongDateString();

                return new BlogViewModel {
                    Title = title,
                    Content = content.Split("---")[1],
                    Published = date
                };
            })),
            new SetDestination(Config.FromDocument((doc, ctx) =>
                {
                    return new NormalizedPath($"blog/{doc.Source.FileNameWithoutExtension}.html");
                })
            )};

        OutputModules = new ModuleList {
            new WriteFiles()
        };
    }
}