using RPG_Game.Controller;
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

// Map configurator is responsible for configuring the proper chain of input keys handling
// Implement a proper MVC model
// Serialize cofiguration and save thm in files
public class MapConfigurator : IConfigurator
{ 
    private Room _room;
    private MapInstructionConfigurator _instructionConfigurator;

    private ItemLists _items = ItemLists.GetInstance();

    private const int _centralRoomWidth = Room._defaultWidth / 5;
    private const int _centralRoomHeight = Room._defaultHeight / 5;

    private const int _spatialGridCellWidth = Room._defaultWidth / 5;
    private const int _spatialGridCellHeight = Room._defaultHeight / 5;

    private int _maxRoomWidth = Math.Min(_centralRoomWidth / 2, _spatialGridCellWidth);
    private int _maxRoomHeight = Math.Min(_centralRoomHeight / 2, _spatialGridCellHeight);

    public MapConfigurator() => this.ResetMapConfiguration();
  
    public void ResetMapConfiguration()
    {
        this._room = new Room();
        this._instructionConfigurator = new MapInstructionConfigurator();
    }

    public Room GetResult() => this._room;
    public MapInstructionConfigurator GetInstructionConfiguration() => this._instructionConfigurator;

    public void CreateEmptyDungeon()
    {

        for (int i = 0; i < Room._width; i++)
            for (int j = 0; j < Room._height; j++)
            {
                _room.RetrieveGrid()[i, j] = new Cell();
                _room.RetrieveGrid()[i, j].CellType = _room.RetrieveGrid()[i, j].CellType | CellType.Empty & ~CellType.Wall;
            }
        this._instructionConfigurator.CreateEmptyDungeon();
    }

    public void FillDungeon()
    {
        for (int i = 0; i < Room._width; i++)
            for (int j = 0; j < Room._height; j++)
                _room.AddObject(CellType.Wall, (i, j));
        this._instructionConfigurator.FillDungeon();
    }

    // Adding paths is possible once the chambers and the central room is generated
    public void AddPaths()
    {
        Random random = new Random();
        int startX = 2 * random.Next(0, Room._width / 2) + 1;
        int startY = 2 * random.Next(0, Room._height / 2) + 1;
        bool[,] visited = new bool[Room._width, Room._height];
        for (int i = 0; i < Room._width; i++)
            for (int j = 0; j < Room._height; j++)
                visited[i, j] = false;

        DFS(startX, startY, visited, random);

        this._instructionConfigurator.AddPaths();
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
            && position.y >= Room._frameSize && position.y < Room._height - Room._frameSize)
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
                int x = random.Next(1 + i * _spatialGridCellWidth, (i + 1) * _spatialGridCellWidth);
                int y = random.Next(1 + j * _spatialGridCellHeight, (j + 1) * _spatialGridCellHeight);

                int width = random.Next(2, _maxRoomWidth);
                int height = random.Next(2, _maxRoomHeight);

                if (x + width >= Room._width || y + height >= Room._height) continue;

                for (int k = x; k < x + width; k++)
                    for (int m = y; m < y + height; m++)
                        _room.RetrieveGrid()[k, m].CellType &= ~CellType.Wall;

                //_roomVertices.Add(new RoomVertex(x, y, width, height));
            }
        }
        this._instructionConfigurator.AddChambers();
    }
    public void AddCentralRoom()
    {
        // + 1 ensures better positioning due to inetegr division
        int left = Room._defaultWidth / 2 - MapConfigurator._centralRoomWidth / 2 + 1;
        int top = Room._defaultHeight / 2 - MapConfigurator._centralRoomHeight / 2 + 1;

        for (int i = left; i < left + MapConfigurator._centralRoomWidth; i++)
            for (int j = top; j < top + MapConfigurator._centralRoomHeight; j++)
                _room.RetrieveGrid()[i, j].CellType &= ~CellType.Wall;

        //_centralRoom = new RoomVertex(left, top, MapBuilder._centralRoomWidth, MapBuilder._centralRoomHeight);
        this._instructionConfigurator.AddCentralRoom();
    }

    // Must work because of covariance
    public void RandomizeItemsPlacement(IEnumerable<IItem> items, int factor)
    {
        Random random = new Random();
        List<IItem> itemList = items.ToList();
        for (int i = 0; i < Room._defaultWidth * Room._defaultHeight / factor; i++)
        {
            int X = Room._frameSize + random.Next() % (Room._defaultWidth - Room._frameSize);
            int Y = Room._frameSize + random.Next() % (Room._defaultHeight - Room._frameSize);

            int randomIndex = random.Next() % itemList.Count;
            _room.AddItem(itemList[randomIndex], (X, Y));
        }
    }

    public void SpawnPlayer(Player player) => player.PlacePlayer(_room);
    public void PlaceItems()
    {
        RandomizeItemsPlacement(_items.ItemList, 15);
        this._instructionConfigurator.PlaceItems();
    }
    public void PlaceDecoratedItems()
    {
        RandomizeItemsPlacement(_items.DecoratedItemList, 20);
        this._instructionConfigurator.PlaceDecoratedItems();
    }
    public void PlaceWeapons()
    {
        RandomizeItemsPlacement(_items.WeaponList, 10);
        this._instructionConfigurator.PlaceWeapons();
    }
    public void PlaceDecoratedWeapons()
    {
        RandomizeItemsPlacement(_items.DecoratedWeaponList, 20);
        this._instructionConfigurator.PlaceDecoratedWeapons();
    }
    public void PlacePotions()
    {
        RandomizeItemsPlacement(_items.PotionList, 10);
        this._instructionConfigurator.PlacePotions();
    }
    public void PlaceEnemies()
    {
        Random random = new Random();
        for(int i = 0; i < Room._defaultWidth * Room._defaultHeight / 20; i++)
        {
            int X = Room._frameSize + random.Next(0, Room._defaultWidth);
            int Y = Room._frameSize + random.Next(0, Room._defaultHeight);

            int randomIndex = random.Next(0, _items.EnemyList.Count);
            _room.AddEnemy(_items.EnemyList[randomIndex], (X, Y));
        }

        this._instructionConfigurator.PlaceEnemies();
    }
}


//for (int i = 0; i < _roomVertices.Count; i++)
//    ConnectRooms(_roomVertices[i], _centralRoom);

//void ConnectRooms(RoomVertex room1, RoomVertex room2)
//{
//    Random random = new Random();
//    (int x1, int y1) = (room1.left + random.Next(0, room1.width), room1.top + random.Next(0, room1.height));

//    (int x2, int y2) = (room2.left + random.Next(room2.width / 2 - 1, room2.width / 2), room2.top + random.Next(room2.height / 2, room2.height / 2 + 1));

//    // We connect any two points inside the room
//    while (x1 != x2 || y1 != y2)
//    {
//        if (random.Next(0, 2) == 1 || y1 == y2)
//        {
//            if (x1 < x2 && random.Next(0, 2) == 1) x1++;
//            else if (x1 > x2) x1--;
//        }
//        else
//        {
//            if (y1 < y2 && random.Next(0, 2) == 1) y1++;
//            else if (y1 > y2) y1--;
//        }
//        _room.RetrieveGrid()[x1, y1].CellType &= ~CellType.Wall;
//    }
//}

//private List<RoomVertex> _roomVertices = new List<RoomVertex>();
//private RoomVertex _centralRoom;