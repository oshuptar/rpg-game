using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

public class MapInstructionConfigurator : IConfigurator
{
    private StringBuilder _sb = new StringBuilder();
    private bool ItemControls = false;
    private bool MoveControls = false;

    public MapInstructionConfigurator() => ResetMapConfiguration();

    public void AddItemControls()
    {
        _sb.Append(" - To pick up an item - press `E`\n");
        _sb.Append(" - To drop an item - press `G`\n");
        _sb.Append(" - To equip/unequip an item - press `Q`. You can equip an item from inventory or from staying on the item, depending on your focus\n");
        _sb.Append(" - Use arrows to navigate through inventory, map items or eqquiped items\n");
        _sb.Append(" - To enter the inventory scope - press `I`, then use arrows to change the object\n");
        _sb.Append(" - To enter hands scope - press `H`, then use arrows to change the object\n");
        _sb.Append(" - To leave hands or inventory scope - press `Escape`\n");

        ObjectRenderer.GetInstance().noOfControlsLines += 7;
    }

    public void AddMoveControls()
    {
        _sb.Append(" - Moves in four directions are controlled by `W`, `S`, `A`, `D`\n");
        ObjectRenderer.GetInstance().noOfControlsLines += 1;
    }

    public void AddCentralRoom()
    {
        if (!MoveControls)
        {
            AddMoveControls();
            MoveControls = true;
        }
    }

    public void AddChambers()
    {
        if (!MoveControls)
        {
            AddMoveControls();
            MoveControls = true;
        }
    }

    public void AddPaths()
    {
        if (!MoveControls)
        {
            AddMoveControls();
            MoveControls = true;
        }
    }

    public void CreateEmptyDungeon()
    {
        _sb.Append("Instructions:\n");
        _sb.Append(" - To open/hide controls - press `K`\n");

        ObjectRenderer.GetInstance().noOfControlsLines += 2;
    }

    public void FillDungeon()
    { }

    public void PlaceDecoratedItems()
    {
        if (!ItemControls && MoveControls)
        {
            AddItemControls();
            ItemControls = true;
        }
    }

    public void PlaceDecoratedWeapons()
    {
        if (!ItemControls && MoveControls)
        {
            AddItemControls();
            ItemControls = true;
        }
        else if (MoveControls)
        {
            _sb.Append(" - Decorated items can change player's attributes once picked up\n");
            ObjectRenderer.GetInstance().noOfControlsLines += 1;
        }
    }

    public void PlaceEnemies()
    {
        // Some info about enemies
    }

    public void PlaceItems()
    {
        if (!ItemControls && MoveControls)
        {
            AddItemControls();
            ItemControls = true;
        }
    }

    public void PlacePotions()
    {
        if (!ItemControls && MoveControls)
        {
            AddItemControls();
            ItemControls = true;
        }
        else if (MoveControls)
        {
            _sb.Append(" - Potions can affect player's attributes\n");
            ObjectRenderer.GetInstance().noOfControlsLines++;
        }
    }

    public void PlaceWeapons()
    {
        if (!ItemControls && MoveControls)
        {
            AddItemControls();
            ItemControls = true;
        }
    }

    public void ResetMapConfiguration()
    {
        _sb = new StringBuilder();
    }

    public StringBuilder GetResult() => _sb;
}

          