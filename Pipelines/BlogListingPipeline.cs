using JonDJones.DotNet.StaticSite.ViewModels;
using Statiq.Razor;
using Statiq.Yaml;

namespace JonDJones.DotNet.StaticSite.Pipelines;

public class BlogListingPipeline : LayoutPipeline {
    public BlogListingPipeline() {

        Dependencies.Add(nameof(BlogPipeline));

        InputModules = new ModuleList
        {
            new ReadFiles("pages/listing.md")
        };

        ProcessModules = new ModuleList {
            new ExtractFrontMatter(
                new ParseYaml()
            ),
            new MergeContent(new ReadFiles(patterns: "BlogListing.cshtml")),
            new RenderRazor().WithModel(Config.FromDocument((document, context) =>
            {
                var title = document.GetString("Title");

                var blogs = context.OutputPages.Where(x => x.Destination.FullPath.Contains("blog/"));
                var blogSummaries = new List<BlogSummaryViewModel>();

                foreach (var blog in blogs)
                {
                    var blogTitle = blog.GetString("Title");
                    var description = blog.GetString("Description");
                    var slug = blog.Source.FileNameWithoutExtension.ToString();

                    var summary = new BlogSummaryViewModel
                    {
                        Title = blogTitle,
                        Description = description,
                        Slug = slug
                    };

                    blogSummaries.Add(summary);
                }
                return new BlogListingViewModel
                {
                    Title = title,
                    Blogs = blogSummaries
                };
            })),
            new SetDestination(Config.FromDocument((doc, ctx) => {
                return new NormalizedPath("listing.html");
            }))
        };
        OutputModules = new ModuleList {
            new WriteFiles()
        };
    }
}