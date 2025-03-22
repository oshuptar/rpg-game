using RPG_Game.Entiities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RPG_Game.Entiities.Room;

namespace RPG_Game.HelperClasses;

public static class ObjectRenderer
{
    public static int noOfControlsLines = 0; // the number of lines to be printed in the Controls decription
    private static MapInstructionConfigurator _mapInstructionConfigurator = new MapInstructionConfigurator();
    public static void SetMapInstructionConfigurator(MapInstructionConfigurator mapIstructions) => _mapInstructionConfigurator = mapIstructions;
    public static StringBuilder RenderItemList(List<IItem> list, string name)
    {
        StringBuilder sb = new StringBuilder();

        string? output = null;
        if (list != null && list.Count != 0)
        {
            // On each element of the sequence the lambda function is called
            output = string.Join(", ", list.Select(item => item.Name));
        }
        // Displays none if output == null
        return sb.Append(String.Format($"{name}: {output ?? "None"}"));
    }

    public static StringBuilder RenderGrid(Room room)
    {
        StringBuilder sb = new StringBuilder();
        Cell[,] grid = room.RetrieveGrid();

        for (int i = 0; i < Room._height; i++)
        {
            for (int j = 0; j < Room._width; j++)
            {
                sb.Append(grid[j, i].PrintCell());
            }
            sb.Append('\n');
        }
        return sb;
    }

    public static StringBuilder RenderControls(bool isControlsVisible)
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

    public static StringBuilder RenderEmptyLine()
    {
        StringBuilder sb = new StringBuilder();

        (int left, int top) = Console.GetCursorPosition();
        sb.Append("".PadRight(Console.WindowWidth - left - 1, ' ')); //??? check the behavior with empty spaces and underscores
        return sb;
    }
}
