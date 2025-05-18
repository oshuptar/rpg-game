using RPG_Game.Controller;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.Interfaces;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Model;

public interface IGameState
{
    public int PlayerId { get; set; }
    public Player GetPlayer();
    public List<Entity> GetVisibleEntities();
    public List<Item>? GetItems(Position pos);
    public StringBuilder RenderMap();
}
