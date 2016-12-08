using System;

namespace PuzzleChart.Api.Interfaces
{
    public interface IPlugin
    {
        String Name { get; set; }
        IPluginHost Host { get; set; }
    }
}
