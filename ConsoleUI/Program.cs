using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Application application = new Application();
            application.AddCommand(new ExitCommand(application));
            application.AddCommand(new HelpCommand(application));
            application.Run();
        }
    }
}
