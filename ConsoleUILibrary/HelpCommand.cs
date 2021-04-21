using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class HelpCommand : ICommand
    {
        Application app;
        public string Name => "help";
        public string Help => "Краткая помощь по всем командам";
        public string[] Synonyms
        {
            get { return new string[] { "?" }; }
        }
        public string Description => "Выводит список  команд с краткой помощью";

        public HelpCommand(Application app)
        {
            this.app = app;
        }
        public void Execute(params string[] parameters)
        {
            if (parameters.Length < 1)
            {
                Console.WriteLine(line);
                foreach (ICommand cmd in app.Commands)
                {
                    Console.WriteLine($"{cmd.Name}: {cmd.Help}");
                }
                Console.WriteLine(line);
            }
            else
            {
                Console.WriteLine(app.FindCommand(parameters[0]).Description);
            }
        }
        private const string line = "================================================";

    }
}
