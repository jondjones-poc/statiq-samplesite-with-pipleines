namespace JonDJones;

public class Program {
    public static async Task<int> Main(string[] args) =>
         await Bootstrapper
            .Factory
            .CreateDefault(args)
            .AddHostingCommands()
            .RunAsync();
}