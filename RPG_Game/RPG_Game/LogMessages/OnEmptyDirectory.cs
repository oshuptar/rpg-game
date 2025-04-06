using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.LogMessages;

public class OnEmptyDirectory
{
    public string Name { get; set; }
    public OnEmptyDirectory(string name)
    {
        Name = name;
    }
}
