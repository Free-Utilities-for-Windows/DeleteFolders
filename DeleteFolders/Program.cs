using System.CommandLine;
using System.CommandLine.Invocation;

class Program
{
    static void Main(string[] args)
    {
        var cmd = new RootCommand
        {
            new Option<string>(
                "--folder",
                description: "Folder to zap."),
            new Option<bool>(
                "--recursive",
                getDefaultValue: () => false,
                description: "Perform a recursive delete.")
        };

        cmd.Handler = CommandHandler.Create<string, bool>((folder, recursive) =>
        {
            if (string.IsNullOrEmpty(folder))
            {
                Console.WriteLine("Error: folder must be supplied");
                return;
            }

            if (Directory.Exists(folder))
            {
                try
                {
                    Directory.Delete(folder, recursive);
                    Console.WriteLine("INFO: Zapped successfully");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("ERROR: You do not have permission to delete this folder. " + ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine("ERROR: The directory was not found. " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("ERROR: Folder does not exist");
            }
        });

        cmd.InvokeAsync(args).Wait();
    }
}