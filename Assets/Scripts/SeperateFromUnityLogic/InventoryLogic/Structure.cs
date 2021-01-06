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
        public WallPlacement(Item it, GridPosition start, GridPosition end, int floor, bool stretch = false)
        {
            item = it;
            StartPoint = start;
            EndPoint = end;
            Floor = floor;
            Stretch = stretch;
        }

        public Item item { get; private set; }
        public int Floor { get; private set; }
        public GridPosition StartPoint { get; private set; }
        public GridPosition EndPoint { get; private set; }
        public bool Stretch { get; private set; }

        public void SetAsStretch(bool stretch)
        {
            Stretch = stretch;
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

        public GridPosition FindPartPosition(Item part)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if(walls[i].item == part)
                {
                    return walls[i].StartPoint;
                }
            }
            return null;
        }

        public Item ItemFromWallPlacement(WallPlacement wallPlacement)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if(walls[i].StartPoint.X == wallPlacement.StartPoint.X && walls[i].StartPoint.Y == wallPlacement.StartPoint.Y && walls[i].EndPoint.X == wallPlacement.EndPoint.X && walls[i].EndPoint.Y == wallPlacement.EndPoint.Y)
                {
                    return walls[i].item;
                }
            }
            return null;
        }

        public WallPlacement WallOnPosition(GridPosition startPosition, GridPosition endPosition)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                if(walls[i].StartPoint == startPosition && walls[i].EndPoint == endPosition)
                {
                    return walls[i];
                }
            }
            return null;
        }

        public void RemoveWall(WallPlacement wall)
        {
            walls.Remove(wall);
        }

        public void RemoveFloor(FloorPlacement floor)
        {
            floors.Remove(floor);
        }



        public void AddPartAsWall(int startX, int startY, int endX, int endY, Item item, int floor, bool stretch = false)
        {
            GridPosition start = new GridPosition(startX, startY);
            GridPosition end = new GridPosition(endX, endY);

            AddPartAsWall(start, end, item, floor);
        }


        public void AddPartAsWall(GridPosition start, GridPosition end, Item item, int floor, bool stretch = false)
        {
            WallPlacement wall = new WallPlacement(item, start, end, floor);
            AddPartAsWall(wall);

        }

        public void AddPartAsWall(WallPlacement wall, bool stretch = false)
        {
            wall.SetAsStretch(stretch);
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
