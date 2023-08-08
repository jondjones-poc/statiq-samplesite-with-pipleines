namespace JonDJones.DotNet.StaticSite.Pipelines;

public class LayoutPipeline : Pipeline {
    public LayoutPipeline() {
        Dependencies.Add(nameof(MenuPipeline));
    }
}