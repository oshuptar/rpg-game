using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

public class ObjectRenderer
{
    public int noOfControlsLines = 0; // the number of lines to be printed in the Controls decription
    private static ObjectRenderer? _objectRendererInstance;
    private MapInstructionConfigurator _mapInstructionConfigurator = new MapInstructionConfigurator();
    public static ObjectRenderer GetInstance()
    {
        if (_objectRendererInstance == null)
            _objectRendererInstance = new ObjectRenderer();
        return _objectRendererInstance;
    }
    private ObjectRenderer() { }
    public void SetMapInstructionConfigurator(MapInstructionConfigurator mapIstructions) => _mapInstructionConfigurator = mapIstructions;
    public StringBuilder RenderItemList(List<Item>? list, string name)
    {
        StringBuilder sb = new StringBuilder();

        string? output = null;
        if (list != null && list.Count != 0)
            output = string.Join(", ", list.Select(item => item.Name));
        return sb.Append(String.Format($"{name}: {output ?? "None"}"));
    }
    public StringBuilder RenderMap(IGameState gameState)
    {
        return gameState.RenderMap();
    }
    public StringBuilder RenderControls(bool isControlsVisible)
    {
        StringBuilder sb = new StringBuilder();
        if(isControlsVisible)
            sb = _mapInstructionConfigurator.GetResult();
        else
        {
            for(int i = 0;i < noOfControlsLines; i++)
                sb.Append(RenderEmptyLine());
        }
        return sb;
    }
    public StringBuilder RenderEmptyLine()
    {
        StringBuilder sb = new StringBuilder();
        (int left, int top) = Console.GetCursorPosition();
        sb.Append("".PadRight(Console.WindowWidth - left - 1, ' ')); //??? check the behavior with empty spaces and underscores
        return sb;
    }
}
