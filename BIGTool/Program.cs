using System.CommandLine;

namespace BIGTool;

internal class Program
{
    private static Task<int> Main(string[] args)
    {
        var bigfileOption = new Option<FileInfo?>(
            name: "--file",
            description: "The path to the BIG file.",
            isDefault: true,
            parseArgument: result =>
            {
                if (result.Tokens.Count == 0)
                {
                    return new FileInfo("CCCPSXP.BIG");
                }
                string? filePath = result.Tokens.Single().Value;
                if (!File.Exists(filePath))
                {
                    result.ErrorMessage = "File does not exist";
                    return null;
                }
                else
                {
                    return new FileInfo(filePath);
                }
            });

        var rootCommand = new RootCommand("Command-line application for modifying Hot Wheels Turbo Racing BIG files for the PS1.");
        var infoCommand = new Command("info", "Displays information about the file.") { bigfileOption };
        rootCommand.AddCommand(infoCommand);

        infoCommand.SetHandler(async (file) =>
            {
                await ReadBigFile(file!);
            },
            bigfileOption);

        return Task.FromResult(rootCommand.InvokeAsync(args).Result);
    }

    private static Task ReadBigFile(
        FileInfo file)
    {
        List<string> lines = File.ReadLines(file.FullName).ToList();
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        };
        return Task.CompletedTask;
    }
}