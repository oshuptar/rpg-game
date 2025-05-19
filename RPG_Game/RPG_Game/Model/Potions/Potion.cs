using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Model.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RPG_Game.Interfaces;
public abstract class Potion : Item
{
    [JsonInclude]
    public bool IsDisposed { get; protected set; } = false;
    public abstract void ApplyEffect(EntityStats? entityStats);
    public abstract void Dispose(Entity? entity);
}
