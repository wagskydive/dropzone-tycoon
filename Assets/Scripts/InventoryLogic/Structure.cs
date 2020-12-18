using System.Collections.Generic;

namespace InventoryLogic
{
    public struct GridPosition
    {
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; private set; }
        public int Y { get; private set; }
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

    public class Structure
    {
        public float GridSize { get; private set; }
        public float StorySize { get; private set; }


        public List<Item> parts { get; private set; }
        public List<WallPlacement> walls { get; private set; }
        public List<FloorPlacement> floors { get; private set; }

        public Structure(float gridSize, float storySize)
        {
            GridSize = gridSize;
            StorySize = storySize;
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
            walls.Add(wall);
            AddPart(item);
        }

        public void AddPartAsFloor(GridPosition gridPosition, Item item, int floor)
        {
            FloorPlacement floorPlacement = new FloorPlacement(item, gridPosition, floor);
            floors.Add(floorPlacement);
            AddPart(item);
        }
    }
}
