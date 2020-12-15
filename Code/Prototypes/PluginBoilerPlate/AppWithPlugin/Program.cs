using CygSoft.Waxy.PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CygSoft.Waxy.CmdConsole
{
    /*
     * Create a .NET Core application with plugins
     * https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
     * 
     * Important! Currently the HelloPlugin needs to be copied to your AppWithPlugin project in order to run. Every change will need to be copied.
     * */
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && args[0] == "/d")
                {
                    Console.WriteLine("Waiting for any key...");
                    Console.ReadLine();
                }

                var commands = LoadCommandsFromPlugins(new string[] { @"HelloPlugin\bin\Debug\net5.0\HelloPlugin.dll" });

                if (args.Length == 0)
                {
                    Console.WriteLine("Commands: ");
                    OutputLoadedCommands(commands);
                }
                else
                {
                    foreach (string commandName in args)
                    {
                        Console.WriteLine($"-- {commandName} --");
                        ExecuteCommand(commands, commandName);

                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static IEnumerable<ICommand> LoadCommandsFromPlugins(string[] pluginPaths)
        {
            IEnumerable<ICommand> commands = pluginPaths.SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateCommands(pluginAssembly);
            }).ToList();

            return commands;
        }

        private static void OutputLoadedCommands(IEnumerable<ICommand> commands)
        {
            foreach (ICommand command in commands)
            {
                Console.WriteLine($"{command.Name}\t - {command.Description}");
            }
        }

        private static void ExecuteCommand(IEnumerable<ICommand> commands, string commandName)
        {
            ICommand command = commands.FirstOrDefault(c => c.Name == commandName);
            if (command == null)
            {
                Console.WriteLine("No such command is known.");
                return;
            }

            command.Execute();
        }

        static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        private static IEnumerable<ICommand> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                {
                    ICommand result = Activator.CreateInstance(type) as ICommand;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}