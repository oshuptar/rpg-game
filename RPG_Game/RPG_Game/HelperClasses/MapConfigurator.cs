using RPG_Game.Controller;
using RPG_Game.Currencies;
using RPG_Game.Entiities;
using RPG_Game.Entities;
using RPG_Game.Enums;
using RPG_Game.HelperClasses;
using RPG_Game.Interfaces;
using RPG_Game.Model;
using RPG_Game.Model.Entities;
using RPG_Game.UnusableItems;
using RPG_Game.Weapons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.HelperClasses;

// Map configurator is responsible for configuring the proper chain of input keys handling
// Implement a proper MVC model
// Serialize cofiguration and save thm in files
public class MapConfigurator : IConfigurator
{ 
    private Room _room;
    private MapInstructionConfigurator _instructionConfigurator;

    private ItemLists _items = ItemLists.GetInstance();

    private const int _centralRoomWidth = MapSettings.DefaultWidth / 5;
    private const int _centralRoomHeight = MapSettings.DefaultHeight / 5;

    private const int _spatialGridCellWidth = MapSettings.DefaultWidth / 5;
    private const int _spatialGridCellHeight = MapSettings.DefaultHeight / 5;

    private int _maxRoomWidth = Math.Min(_centralRoomWidth / 2, _spatialGridCellWidth);
    private int _maxRoomHeight = Math.Min(_centralRoomHeight / 2, _spatialGridCellHeight);

    public MapConfigurator() => this.ResetMapConfiguration();
  
    public void ResetMapConfiguration()
    {
        this._room = new Room(new RoomState());
        this._instructionConfigurator = new MapInstructionConfigurator();
    }
    public Room GetResult() => _room;
    public MapInstructionConfigurator GetInstructionConfiguration() => this._instructionConfigurator;
    public void CreateEmptyDungeon()
    {
        for (int i = 0; i < MapSettings.Width; i++)
            for (int j = 0; j < MapSettings.Height; j++)
            {
                _room.GetRoomState().Grid[i, j].CellType = _room.GetRoomState().Grid[i, j].CellType | CellType.Empty & ~CellType.Wall;
            }
        this._instructionConfigurator.CreateEmptyDungeon();
    }
    public void FillDungeon()
    {
        for (int i = 0; i < MapSettings.Width; i++)
            for (int j = 0; j < MapSettings.Height; j++)
                _room.AddObject(CellType.Wall, new Position(i, j));
        this._instructionConfigurator.FillDungeon();
    }
    public void AddPaths()
    {
        Random random = new Random();
        int startX = 2 * random.Next(0, MapSettings.Width / 2) + 1;
        int startY = 2 * random.Next(0, MapSettings.Height / 2) + 1;
        bool[,] visited = new bool[MapSettings.Width, MapSettings.Height];
        for (int i = 0; i < MapSettings.Width; i++)
            for (int j = 0; j < MapSettings.Height; j++)
                visited[i, j] = false;

        DFS(startX, startY, visited, random);

        this._instructionConfigurator.AddPaths();
    }
    public void DFS(int startX, int startY, bool[,] visited, Random random)
    {
        visited[startX, startY] = true;
        _room.GetRoomState().Grid[startX, startY].CellType &= ~CellType.Wall;

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
                _room.GetRoomState().Grid[midPoint.x, midPoint.y].CellType &= ~CellType.Wall;
                DFS(newPosition.x, newPosition.y, visited, random);
            }
        }
    }
    public bool IsInRange((int x, int y)position)
    {
        return _room.IsInRange(new Position(position.x, position.y));
    }
    public void AddChambers()
    {
        Random random = new Random();
        for (int i = 0; i < MapSettings.DefaultWidth / _spatialGridCellWidth; i++)
        {
            for (int j = 0; j < MapSettings.DefaultHeight / _spatialGridCellHeight; j++)
            {
                int x = random.Next(1 + i * _spatialGridCellWidth, (i + 1) * _spatialGridCellWidth);
                int y = random.Next(1 + j * _spatialGridCellHeight, (j + 1) * _spatialGridCellHeight);

                int width = random.Next(2, _maxRoomWidth);
                int height = random.Next(2, _maxRoomHeight);

                if (x + width >= MapSettings.Width || y + height >= MapSettings.Height) continue;

                for (int k = x; k < x + width; k++)
                    for (int m = y; m < y + height; m++)
                        _room.GetRoomState().Grid[k, m].CellType &= ~CellType.Wall;
            }
        }
        this._instructionConfigurator.AddChambers();
    }
    public void AddCentralRoom()
    {
        // + 1 ensures better positioning due to inetegr division
        int left = MapSettings.DefaultWidth / 2 - MapConfigurator._centralRoomWidth / 2 + 1;
        int top =  MapSettings.DefaultHeight / 2 - MapConfigurator._centralRoomHeight / 2 + 1;

        for (int i = left; i < left + MapConfigurator._centralRoomWidth; i++)
            for (int j = top; j < top + MapConfigurator._centralRoomHeight; j++)
                _room.GetRoomState().Grid[i, j].CellType &= ~CellType.Wall;

        this._instructionConfigurator.AddCentralRoom();
    }
    // Must work because of covariance
    public void RandomizeItemsPlacement(IEnumerable<Item> items, int factor)
    {
        Random random = new Random();
        List<Item> itemList = items.ToList();
        for (int i = 0; i < MapSettings.DefaultWidth * MapSettings.DefaultHeight / factor; i++)
        {
            int X = MapSettings.FrameSize + random.Next() % (MapSettings.DefaultWidth - MapSettings.FrameSize);
            int Y = MapSettings.FrameSize + random.Next() % (MapSettings.DefaultHeight - MapSettings.FrameSize);

            int randomIndex = random.Next() % itemList.Count;
            _room.AddItem((Item)itemList[randomIndex].Copy(), new Position(X, Y));
        }
    }
    public void SpawnPlayer(Player player) => _room.AddPlayer(player, new Position(1, 1));
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
        RandomizeItemsPlacement(_items.DecoratedWeaponList, 15);
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
        for(int i = 0; i < MapSettings.DefaultWidth * MapSettings.DefaultHeight / 20; i++)
        {
            int X = MapSettings.FrameSize + random.Next(0, MapSettings.DefaultWidth);
            int Y = MapSettings.FrameSize + random.Next(0, MapSettings.DefaultHeight);

            int randomIndex = random.Next(0, _items.EnemyList.Count);
            Entity? entity = _items.EnemyList[randomIndex];
            _room.AddEntity((Entity)entity.Copy(), new Position(X, Y));
        }
        this._instructionConfigurator.PlaceEnemies();
    }
}