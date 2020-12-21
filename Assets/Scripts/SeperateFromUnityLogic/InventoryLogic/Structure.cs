using SpawnLogic;
using System.Collections.Generic;

namespace InventoryLogic
{
    [System.Serializable]
    public class GridPosition
    {


        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; private set; }
        public int Y { get; private set; }


        public int[] GridDistance(GridPosition target)
        {
            int[] result = new int[2];
            result[0] = target.X - X;
            result[1] = target.Y - Y;
            return result;

        }
    }

    public class WallPlacement
    {
        public WallPlacement(Item it, GridPosition start, GridPosition end, int floor)
        {
            item = it;
            StartPoint = start;
            EndPoint = end;
            Floor = floor;

        }

        public Item item { get; private set; }
        public int Floor { get; private set; }
        public GridPosition StartPoint { get; private set; }
        public GridPosition EndPoint { get; private set; }
    }

    public class WallSet
    {
        public ItemType Single { get; private set; }
        public ItemType Start { get; private set; }
        public ItemType End { get; private set; }
        public ItemType Middle { get; private set; }

        public WallSet(ItemType single, ItemType start, ItemType end, ItemType middle)
        {

        }
    }



    public class FloorPlacement
    {
        public FloorPlacement(Item it, GridPosition position, int floor)
        {
            item = it;
            gridPosition = position;
            Floor = floor;

        }
        public Item item { get; private set; }
        public GridPosition gridPosition { get; private set; }
        public int Floor { get; private set; }
    }
    [System.Serializable]
    public class Structure : ISpawnable
    {
        public string Name { get; private set; }

        public float GridSize { get; private set; }
        public float FloorSize { get; private set; }


        public List<Item> parts { get; private set; }
        public List<WallPlacement> walls { get; private set; }
        public List<FloorPlacement> floors { get; private set; }

        public Structure(string name, float gridSize, float floorSize)
        {
            Name = name;
            GridSize = gridSize;
            FloorSize = floorSize;
            parts = new List<Item>();
            walls = new List<WallPlacement>();
            floors = new List<FloorPlacement>();
        }

        public void AddPart(Item item)
        {
            parts.Add(item);
        }

        public void RemovePart(Item item)
        {
            if (parts.Contains(item))
            {
                parts.Remove(item);
                walls.Find(x => x.item == item);
                WallPlacement wall = walls.Find(x => x.item == item);
                if (wall != null)
                {
                    RemoveWall(wall);
                }
                FloorPlacement floor = floors.Find(x => x.item == item);
                if (floor != null)
                {
                    RemoveFloor(floor);
                }
            }
        }



        public void RemoveWall(WallPlacement wall)
        {
            walls.Remove(wall);
        }

        public void RemoveFloor(FloorPlacement floor)
        {
            floors.Remove(floor);
        }



        public void AddPartAsWall(int startX, int startY, int endX, int endY, Item item, int floor)
        {
            GridPosition start = new GridPosition(startX, startY);
            GridPosition end = new GridPosition(endX, endY);

            AddPartAsWall(start, end, item, floor);
        }


        public void AddPartAsWall(GridPosition start, GridPosition end, Item item, int floor)
        {
            WallPlacement wall = new WallPlacement(item, start, end, floor);
            AddPartAsWall(wall);

        }

        public void AddPartAsWall(WallPlacement wall)
        {

            walls.Add(wall);
            AddPart(wall.item);
        }

        public void AddWallRange(GridPosition start, GridPosition end, Item item, int floor, bool overWrite = true)
        {

        }


        public void AddPartAsFloor(FloorPlacement floorPlacement)
        {
            
            floors.Add(floorPlacement);
            AddPart(floorPlacement.item);
        }


        public void AddPartAsFloor(GridPosition gridPosition, Item item, int floor)
        {
            AddPartAsFloor(new FloorPlacement(item, gridPosition, floor));

        }

        public string ResourcePath()
        {
            return "";
        }
    }
}
