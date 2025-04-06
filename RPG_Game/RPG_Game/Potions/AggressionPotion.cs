using RPG_Game.Entities;
using RPG_Game.Interfaces;
using RPG_Game.Entiities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Enums;

namespace RPG_Game.Potions;

// Implement via events in PlayerStats class

//Temporary potion
public class AggressionPotion : IPotion
{
    public string Name => "Aggresion Potion";

    public int Aggression = 5;
    public void ApplyEffect(EntityStats? entityStats) 
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Aggression, Aggression);
    }
    public void RevertEffect(EntityStats? entityStats) 
    {
        entityStats?.ModifyEntityAttribute(PlayerAttributes.Aggression, -Aggression);
    }
    public string Description => $"(Adds {Aggression} Aggression)";

    public void Use(IEntity? entity)
    {
        ApplyEffect(entity?.RetrieveEntityStats()!);
    }

    public void Dispose(IEntity? entity)
    {
        RevertEffect(entity?.RetrieveEntityStats()!);
    }
    public void PickUp(IEntity? entity)
    {

    }

    public void Drop(IEntity? entity)
    {

    }
    public void OnMoveHandler(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
