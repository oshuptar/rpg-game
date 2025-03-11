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
    private static int noOfControlsLines = 10; // the number of lines to be printed in the Controls decription
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
        if (isControlsVisible)
        {
            sb.Append("Controls:\n");
            sb.Append(" - To open/hide controls - press `K`\n");
            sb.Append(" - Moves in four directions are controlled by `W`, `S`, `A`, `D`\n");
            sb.Append(" - To pick up an item - press `E`\n");
            sb.Append(" - To drop an item - press `G`\n");
            sb.Append(" - To equip/unequip an item - press `Q`. You can equip an item from inventory or from staying on the item, depending on your focus\n");
            sb.Append(" - Use arrows to navigate through inventory, map items or eqquiped items\n");
            sb.Append(" - To enter the inventory scope - press `I`, then use arrows to change the object\n");
            sb.Append(" - To enter hands scope - press `H`, then use arrows to change the object\n");
            sb.Append(" - To leave hands or inventory scope - press `Escape`\n");
        }

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
