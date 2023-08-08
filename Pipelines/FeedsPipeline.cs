using Statiq.Feeds;
using Statiq.Markdown;
using Statiq.Yaml;

namespace JonDJones.DotNet.StaticSite.Pipelines;

public class FeedsPipeline : Pipeline {
    public FeedsPipeline() {

        InputModules = new ModuleList
        {
            new ReadFiles("./posts/*.md")
        };

        ProcessModules = new ModuleList
        {
            new ExtractFrontMatter(new ParseYaml()),
            new RenderMarkdown().UseExtensions(),
            new GenerateExcerpt(),
            new OrderDocuments(Config.FromDocument((x => x.GetDateTime(FeedKeys.Published)))).Descending(),
            new GenerateFeeds()
        };
        OutputModules = new ModuleList
        {
            new WriteFiles()
        };
    }
}