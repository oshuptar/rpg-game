using RPG_Game.Entiities;
using RPG_Game.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;

// Categorizing interface
public interface IPotion : IItem
{
    public void OnMoveHandler(object sender, EventArgs e);
    public void RevertEffect(EntityStats? entityStats);
    public void ApplyEffect(EntityStats? entityStats);
    public void Dispose(IEntity? entity);
}
