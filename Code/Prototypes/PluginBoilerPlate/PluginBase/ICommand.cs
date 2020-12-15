using System;
using System.Collections.Generic;
using System.Text;

namespace CygSoft.Waxy.PluginBase
{
    /*
     * This ICommand interface is the interface that all of the plugins will implement.
     * */
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }

        int Execute();
    }
}
