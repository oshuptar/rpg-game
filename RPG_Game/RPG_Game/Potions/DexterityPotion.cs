using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Potions;

// The ability of hero dodging attacks
public class DexterityPotion : IPotion
{
    public string Name => "Dexterity Potion";

    public int Dexterity = 4;
    public string Description => $"(Adds {Dexterity} to the ability of dodging attacks)";

    public void RevertEffect(EntityStats? entityStats)
    {
        throw new NotImplementedException();
    }

    public void ApplyEffect(EntityStats? Stats)
    {
        throw new NotImplementedException();
    }
    public void PickUp(IEntity? entity)
    {

    }

    public void Drop(IEntity? entity)
    {

    }

    public void Use(IEntity? entity)
    {
        ApplyEffect(entity?.RetrieveEntityStats());
    }
    public void Dispose(IEntity? entity)
    {
        RevertEffect(entity?.RetrieveEntityStats());
    }

    public void OnMoveHandler(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
