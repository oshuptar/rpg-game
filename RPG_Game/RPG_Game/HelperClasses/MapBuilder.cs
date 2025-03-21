using RPG_Game.Currencies;
using RPG_Game.Entiities;
using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using RPG_Game.UnusableItems;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static RPG_Game.Entiities.Room;

namespace RPG_Game.HelperClasses;

public class MapBuilder : IBuilder
{
    private static List<IItem> ItemList = new List<IItem>()
    {
        new Gold(),
        new Coin(),
        new Key(),
        new Note(),
        new Lore()
    };

    private static List<IItem> DecoratedItemList = new List<IItem>()
    {
        new LuckItemDecorator(new Key()),
        new LuckItemDecorator(new Gold()),
        new LuckItemDecorator(new Coin()),
        new LuckItemDecorator(new LuckItemDecorator(new Lore())),
        new LuckItemDecorator(new LuckItemDecorator (new Coin())),
    };

    private static List<IItem> WeaponList = new List<IItem>()
    {
        new Hammer(),
        new Sword(),
        new Dagger(),
    };

    private static List<IItem> DecoratedWeaponList = new List<IItem>()
    {
        new LuckItemDecorator(new Sword()),
        new PowerWeaponDecorator(new Hammer()),
        new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer())),
        new LuckItemDecorator( new PowerWeaponDecorator(new PowerWeaponDecorator(new Hammer()))),
        new Hammer(),
        new Sword(),
        new Dagger(),
        new PowerWeaponDecorator(new Dagger())
    };

    private Room _room;
    private const int _centralRoomWidth = Room._defaultWidth / 8;
    private const int _centralRoomHeight = Room._defaultHeight / 8;

    private const int _spatialGridCellWidth = Room._defaultWidth / 7;
    private const int _spatialGridCellHeight = Room._defaultHeight / 7;

    private int _maxRoomWidth = Math.Min(_centralRoomWidth - 1, _spatialGridCellWidth);
    private int _maxRoomHeight = Math.Min(_centralRoomHeight - 1, _spatialGridCellHeight);

    private List<RoomVertex> _roomVertices = new List<RoomVertex>();

    private RoomVertex _centralRoom;
    public MapBuilder() => this.ResetMapConfiguration();

    public void ResetMapConfiguration() => this._room = new Room();

    public Room GetResult()
    {
        Room temp = this._room;
        this.ResetMapConfiguration();
        return temp;
    }

    public void CreateEmptyDungeon()
    {
        for (int i = 0; i < Room._width; i++)
            for (int j = 0; j < Room._height; j++)
            {
                _room.RetrieveGrid()[i, j] = new Cell();
                _room.RetrieveGrid()[i, j].CellType = _room.RetrieveGrid()[i, j].CellType | CellType.Empty & ~CellType.Wall;
            }
    }

    public void FillDungeon()
    {
        for (int i = 0; i < Room._width; i++)
            for (int j = 0; j < Room._height; j++)
                _room.AddObject(CellType.Wall, (i, j));
    }

    void ConnectRooms(RoomVertex room1, RoomVertex room2)
    {
        Random random = new Random();
        (int x1, int y1) = (room1.left + random.Next(0, room1.width), room1.top + random.Next(0, room1.height));

        (int x2, int y2) = (room2.left + random.Next(room2.width/2 - 1, room2.width/2), room2.top + random.Next(room2.height/2, room2.height/2 + 1));

        // We connect any two points inside the room
        while (x1 != x2 || y1 != y2)
        {
            if (random.Next(0, 2) == 1 || y1 == y2)
            {
                if (x1 < x2 && random.Next(0, 2) == 1) x1++;
                else if (x1 > x2) x1--;
            }
            else
            {
                if (y1 < y2 && random.Next(0, 2) == 1) y1++;
                else if (y1 > y2) y1--;
            }
            _room.RetrieveGrid()[x1, y1].CellType &= ~CellType.Wall;
        }
    }
    // Adding paths is possible once the chambers and the central room is generated
    public void AddPaths()
    {
        //for (int i = 0; i < _roomVertices.Count; i++)
        //    ConnectRooms(_roomVertices[i], _centralRoom);
        Random random = new Random();
        int startX = 2 * random.Next(0, Room._width / 2) + 1;
        int startY = 2 * random.Next(0, Room._height / 2) + 1;
        bool[,] visited = new bool[Room._width, Room._height];
        for (int i = 0; i < Room._width; i++)
            for (int j = 0; j < Room._height; j++)
                visited[i, j] = false;

        DFS(startX, startY, visited, random);
    }

    public void DFS(int startX, int startY, bool[,] visited, Random random)
    {
        visited[startX, startY] = true;
        _room.RetrieveGrid()[startX, startY].CellType &= ~CellType.Wall;

        List<(int x, int y)> directions = new List<(int, int)> { (-2, 0), (2, 0), (0, -2), (0, 2) };
        // Shuffles a list
        for (int i = directions.Count - 1; i >= 0; i--)
        {
            int idx = random.Next(0, i);
            (directions[idx], directions[i]) = (directions[i], directions[idx]);
        }

        foreach ((int x, int y) direction in directions)
        {
            (int x, int y)newPosition = (startX + direction.x, startY + direction.y);

            if (IsInRange((newPosition.x, newPosition.y)) && !visited[newPosition.x, newPosition.y])
            {
                (int x, int y)midPoint = (startX + (direction.x)/2,  startY + (direction.y)/2);
                _room.RetrieveGrid()[midPoint.x, midPoint.y].CellType &= ~CellType.Wall;
                DFS(newPosition.x, newPosition.y, visited, random);
            }
        }
        
    }

    public bool IsInRange((int x, int y)position)
    {
        if (position.x >= Room._frameSize && position.x < Room._width - Room._frameSize
            && position.y >= Room._frameSize & position.y < Room._height - Room._frameSize)
            return true;
        return false;
    }
    public void AddChambers()
    {
        Random random = new Random();
        for (int i = 0; i < _defaultWidth / _spatialGridCellWidth; i++)
        {
            for (int j = 0; j < _defaultHeight / _spatialGridCellHeight; j++)
            {
                int x = random.Next(i * _spatialGridCellWidth + 1, (i + 1) * _spatialGridCellWidth + 1);
                int y = random.Next(j * _spatialGridCellHeight + 1, (j + 1) * _spatialGridCellHeight + 1);

                int width = random.Next(2, _maxRoomWidth + 1);
                int height = random.Next(2, _maxRoomHeight + 1);

                if (x + width >= Room._width || y + height >= Room._height) continue;

                for (int k = x; k < x + width; k++)
                    for (int m = y; m < y + height; m++)
                        _room.RetrieveGrid()[k, m].CellType &= ~CellType.Wall;

                _roomVertices.Add(new RoomVertex(x, y, width, height));
            }
        }
    }
    public void AddCentralRoom()
    {
        int left = Room._defaultWidth / 2 - MapBuilder._centralRoomWidth / 2;
        int top = Room._defaultHeight / 2 - MapBuilder._centralRoomHeight / 2;

        for (int i = left; i < left + MapBuilder._centralRoomWidth; i++)
            for (int j = top; j < top + MapBuilder._centralRoomHeight; j++)
                _room.RetrieveGrid()[i, j].CellType &= ~CellType.Wall;

        _centralRoom = new RoomVertex(left, top, MapBuilder._centralRoomWidth, MapBuilder._centralRoomHeight);
    }

    public void RandomizeItemsPlacement(List<IItem> items)
    {
        Random random = new Random();
        for (int i = 0; i < Room._defaultWidth * Room._defaultHeight / 8; i++)
        {
            int X = Room._frameSize + random.Next() % (Room._defaultWidth - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (Room._defaultHeight - Room._frameSize);

            int randomIndex = random.Next() % items.Count;
            _room.AddItem(items[randomIndex], (X, Y));
        }
    }

    public void SpawnPlayer(Player player) => player.PlacePlayer(_room);
    public void PlaceItems() => RandomizeItemsPlacement(ItemList);
    public void PlaceDecoratedItems() => RandomizeItemsPlacement(DecoratedItemList);
    public void PlaceWeapons() => RandomizeItemsPlacement(WeaponList);
    public void PlaceDecoratedWeapons() => RandomizeItemsPlacement(DecoratedWeaponList);
    public void PlacePotions()
    {

    }
    public void PlaceEnemies()
    {

    }
}