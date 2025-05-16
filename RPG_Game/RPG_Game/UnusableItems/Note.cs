using RPG_Game.Controller;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RPG_Game.UIHandlers;

namespace RPG_Game.UnusableItems;

public class Note : ILoot
{
    public override string Name => "A mysterious note";
    public override void Use(AttackStrategy strategy, IEntity? source, List<IEntity>? target) => ConsoleView.GetInstance().LogMessage("Hmm.. Exploration - is the KEY to finding treasures");
    public override string Description => "(Ancient note)";
    public override object Copy() => new Note();
}
