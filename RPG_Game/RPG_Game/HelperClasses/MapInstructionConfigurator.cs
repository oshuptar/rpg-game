using RPG_Game.Controller;
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
        ObjectRenderer.GetInstance().noOfControlsLines += 10;
        _sb.Append(" - Attack - Press 'Space'. Two-Weapon Attack - Press `Shift + Space`\n");
        _sb.Append(" - Normal Attack - `1`. Stealth Attack - `2`. Magic Attack - `3`\n");
        _sb.Append(" - Pick Up - Press `E`\n");
        _sb.Append(" - Drop - Press `G`. Drop All - Press `Ctrl + G`\n");
        _sb.Append(" - Use - Press `U`\n");
        _sb.Append(" - Equip/Unequip - Press `Q`\n");
        _sb.Append(" - Navigation - Via Arrows\n");
        _sb.Append(" - Inventory Score - Press `I`\n");
        _sb.Append(" - Hands Scope - Press `H`\n");
        _sb.Append(" - Leave Current Scope - Press `Escape`\n");

        RequestHandlerChain.GetInstance().AddHandler(new EquipItemHandler());
        RequestHandlerChain.GetInstance().AddHandler(new PickUpItemHandler());
        RequestHandlerChain.GetInstance().AddHandler(new UseItemHandler());
        RequestHandlerChain.GetInstance().AddHandler(new DropItemHandler());
        RequestHandlerChain.GetInstance().AddHandler(new EmptyInventoryHandler());
        RequestHandlerChain.GetInstance().AddHandler(new NextItemHandler());
        RequestHandlerChain.GetInstance().AddHandler(new PrevItemHandler());
        RequestHandlerChain.GetInstance().AddHandler(new ScopeInventoryHandler());
        RequestHandlerChain.GetInstance().AddHandler(new ScopeHandsHandler());
        RequestHandlerChain.GetInstance().AddHandler(new ScopeRoomHandler());
        RequestHandlerChain.GetInstance().AddHandler(new OneWeaponAttackHandler());
        RequestHandlerChain.GetInstance().AddHandler(new TwoWeaponAttackHandler());
        RequestHandlerChain.GetInstance().AddHandler(new MagicAttackModeHandler());
        RequestHandlerChain.GetInstance().AddHandler(new NormalAttackModeHandler());
        RequestHandlerChain.GetInstance().AddHandler(new StealthAttackModeHandler());
    }
    public void AddMoveControls()
    {
        _sb.Append(" - Move - press `W`, `S`, `A`, `D`\n");
        ObjectRenderer.GetInstance().noOfControlsLines += 1;

        RequestHandlerChain.GetInstance().AddHandler(new MoveUpHandler());
        RequestHandlerChain.GetInstance().AddHandler(new MoveDownHandler());
        RequestHandlerChain.GetInstance().AddHandler(new MoveLeftHandler());
        RequestHandlerChain.GetInstance().AddHandler(new MoveRightHandler());
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
        _sb.Append(" - Finish Game - Press `X`\n");
        _sb.Append(" - Open/Hide Controls - Press `K`\n");
        ObjectRenderer.GetInstance().noOfControlsLines += 3;

        RequestHandlerChain.GetInstance().AddHandler(new HideControlsHandler());
        RequestHandlerChain.GetInstance().AddHandler(new QuitHadler());
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
        if(MoveControls)
        {
            _sb.Append(" - There are enemies, which protect treasures. Watch out!\n");
            ObjectRenderer.GetInstance().noOfControlsLines++;
        }
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
          