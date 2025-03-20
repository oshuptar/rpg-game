using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

public interface IBuilder
{
    void ResetMapConfiguration();
    void CreateEmptyDungeon();
    void FillDungeon();
    void AddPaths();
    void AddChambers();
    void AddCentralRoom();
    void PlaceItems();
    void PlaceDecoratedItems();
    void PlaceWeapons();
    void PlaceDecoratedWeapons();
    void PlacePotions();
    void PlaceEnemies();

}
