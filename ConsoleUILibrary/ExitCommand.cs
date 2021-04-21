using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class ExitCommand : ICommand
    {
        Application app;
        public ExitCommand(Application app)
        {
            this.app = app;
        }
        public string Name => "exit";
        public string Help => "Выход из программы";
        public string[] Synonyms => new string[] { "quit", "bye" };
        public string Description => "Длинное и подробное описание команды выхода ";
        public void Execute(params string[] parameters)
        {
            Environment.Exit(0);
        }
    }
}
