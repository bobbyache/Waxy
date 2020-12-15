
using CygSoft.Waxy.PluginBase;
using System;

namespace CygSoft.Waxy.WvScape
{
    public class HelloCommand : ICommand
    {
        public string Name { get => "hello"; }
        public string Description { get => "Displays hello message."; }

        public int Execute()
        {
            Console.WriteLine("Hello !!!");
            return 0;
        }
    }
}