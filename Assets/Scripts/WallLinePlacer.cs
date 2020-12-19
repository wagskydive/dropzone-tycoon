using InventoryLogic;
using SpawnLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WallLinePlacer : ItemPlacer
{
    [SerializeField]
    public StructureObject workingStructure;

    GridPosition startPoint;
    int lastLongestDistForLine = 0;
    bool lastXforLine = true;

    int currentFloor;

    public override event Action OnItemPlaced;
    public override event Action<ISpawnable, Transform, Transform> OnSpawnRequest;
    public event Action<WallPlacement> OnWallPlaced;

    ISpawnable currentStartObjectToAdd;
    ISpawnable currentEndObjectToAdd;
    ISpawnable currentSingleObjectToAdd;


    List<WallPlacement> tempWalls = new List<WallPlacement>();



    public override void Awake()
    {
        base.Awake();
        if (workingStructure != null)
        {
            workingStructure.AssignSpawner(spawner);
        }
    }

    void AdjustListLenghtForLine(int longestDist)
    {
        int visablePlaceholders = Math.Abs(longestDist);
        int placeCount = placeholderGameObjects.Count;
        if (placeCount < visablePlaceholders)
        {
            for (int i = 0; i < visablePlaceholders - placeCount; i++)
            {
                GridPosition gP = new GridPosition(startPoint.X + longestDist - 1, startPoint.Y);

                GameObject go = InstantiatePlaceHolder(currentSpawnable, workingStructure.transform, GridPositionToVector3(gP));
                tempWalls.Add(new WallPlacement((Item)currentSpawnable, new GridPosition(0, 0), new GridPosition(0, 0), currentFloor));

                placeholderGameObjects.Add(go);
            }
        }
    }

    void HandleLinePlacement(Vector3 pointerPos)
    {
        int[] distance = startPoint.GridDistance(Vector3ToGridPosition(pointerPos-workingStructure.transform.position));

        Debug.Log("Distance X: " + distance[0] + "\n" + "Distance Y: " + distance[1]);
        //X Line

        int longestDist = distance[0];
        bool isX = true;


        if (Math.Abs(distance[1]) > Math.Abs(distance[0]))
        {
            longestDist = distance[1];
            isX = false;
        }


        if (longestDist != lastLongestDistForLine || lastXforLine != isX)
        {
            if (longestDist != 0)
            {
                AdjustListLenghtForLine(longestDist);
                HandlePlaceHolderActivations(longestDist);
                HandleObjectSwaps(longestDist);
            }


            HandlePlaceHolderLinePositioning(longestDist, isX);
            HandlePlaceHolderRotation(isX);
        }
        lastLongestDistForLine = longestDist;
        lastXforLine = isX;


    }


    public void SetFirstPlacementObject(ISpawnable objectToAdd, bool stretchWall, ISpawnable startObjectToAdd, ISpawnable endObjectToAdd, ISpawnable singleObjectToAdd, bool snap = true, float grSize = 1)
    {
        currentStartObjectToAdd = startObjectToAdd;
        currentEndObjectToAdd = endObjectToAdd;
        currentSingleObjectToAdd = singleObjectToAdd;
        SetFirstPlacementObject(objectToAdd, snap, grSize);
    }
    public override void SetFirstPlacementObject(ISpawnable objectToAdd, bool snap = false, float grSize = 1)
    {
        base.SetFirstPlacementObject(objectToAdd, snap, grSize);
        GameObject go = InstantiatePlaceHolder(objectToAdd, gameObject.transform);
        DestroyListComplete();
        placeholderGameObjects.Add(go);
        MouseDetect.OnLeftClickDetected += FirstClick;
    }

    void SwapPlaceHolder(int i, ISpawnable replacement)
    {
        GameObject go = InstantiatePlaceHolder(replacement, gameObject.transform);
        go.transform.localPosition = placeholderGameObjects[i].transform.localPosition;
        go.transform.localRotation = placeholderGameObjects[i].transform.localRotation;
        Destroy( placeholderGameObjects[i].gameObject);
        placeholderGameObjects[i] = go;
    }

    public override void FirstClick(Vector3 pos)
    {
        base.FirstClick(pos);
        startPoint = Vector3ToGridPosition(pos - workingStructure.transform.position);
        placeholderGameObjects[0].transform.SetParent(workingStructure.transform);


        MouseDetect.OnLeftClickDetected += ConfirmPlacementRequest;
        MouseDetect.OnOverDetected += HandleLinePlacement;
    }

    public override void CancelPlacement(Vector3 pointerPos)
    {
        base.CancelPlacement(pointerPos);
        MouseDetect.OnLeftClickDetected -= ConfirmPlacementRequest;
        MouseDetect.OnRightClickDetected -= CancelPlacement;
        MouseDetect.OnOverDetected -= HandleLinePlacement;
    }

    public override void ConfirmPlacementRequest(Vector3 pointerPos)
    {
        // MOVE THIS TO LINES
        MouseDetect.OnLeftClickDetected -= ConfirmPlacementRequest;
        MouseDetect.OnOverDetected -= HandleLinePlacement;


        int amount = Math.Abs(lastLongestDistForLine);
        for (int i = 0; i < amount; i++)
        {
            placeholderGameObjects[i].SetActive(false);



            //spawner.Spawn(currentSpawnable, position);

            OnSpawnRequest?.Invoke(currentSpawnable, placeholderGameObjects[i].transform,workingStructure.transform);

            OnWallPlaced?.Invoke(tempWalls[i]);
            OnItemPlaced?.Invoke();
        }
        base.ConfirmPlacementRequest(pointerPos);
    }

    private void HandleObjectSwaps(int longestDist)
    {
        for (int i = 0; i < longestDist; i++)
        {
            if(i == 0)
            {
                SwapPlaceHolder(i, currentStartObjectToAdd);
            }
            if(i == longestDist-1)
            {
                SwapPlaceHolder(i, currentEndObjectToAdd);
            }
        }
    }

    public void SetWorkingStructure(ItemSpawner itemSpawner, StructureObject structure, int floor)
    {
        currentFloor = floor;
        workingStructure = structure;
        workingStructure.AssignSpawner(itemSpawner);
    }

    private WallPlacement WallPlacementFromOffsetVector(Item item, Vector3 OffsetVector, Vector3 direction)
    {
        Vector3 norm = MakePositive(OffsetVector.normalized);

        GridPosition localStartPoint = Vector3ToGridPosition(GridPositionToVector3(startPoint) + OffsetVector);
        GridPosition localEndPoint = Vector3ToGridPosition(GridPositionToVector3(startPoint) + OffsetVector + direction);
        WallPlacement wallPlacement = new WallPlacement(item, localStartPoint, localEndPoint, currentFloor);
        return wallPlacement;
    }

    private void HandlePlaceHolderLinePositioning(int longestDist, bool isX)
    {
        for (int i = 0; i < longestDist; i++)
        {
            Item item;
            if (Math.Abs(longestDist) != 1)
            {
                if (i == 0)
                {
                    item = (Item)currentStartObjectToAdd;
                    SwapPlaceHolder(i, item);
                }
                else if (i == longestDist - 1)
                {
                    item = (Item)currentEndObjectToAdd;
                    SwapPlaceHolder(i, item);
                }
                else
                {
                    item = (Item)currentSpawnable;
                }
            }
            else
            {
                item = (Item)currentSingleObjectToAdd;
            }


            if (isX)
            {
                if (longestDist > 0)
                {


                    
                    WallPlacement wallPlacement = WallPlacementFromOffsetVector(item,Vector3.right*i, Vector3.right);

                    tempWalls[i] = wallPlacement;

                    placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(i, 0, 0);//MakePositive(placeholderGameObjects[i].transform.localPosition);
                }
                else
                {
                    WallPlacement wallPlacement = WallPlacementFromOffsetVector(item,Vector3.left * i, Vector3.right);

                    tempWalls[i] = wallPlacement;

                    placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(-i - 1, 0, 0);//MakePositive(placeholderGameObjects[i].transform.localPosition);
                }
            }
            else
            {
                if (longestDist > 0)
                {
                    WallPlacement wallPlacement = WallPlacementFromOffsetVector(item, Vector3.forward * i, Vector3.forward);

                    tempWalls[i] = wallPlacement;


                    placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(0, 0, i);//MakePositive(placeholderGameObjects[i].transform.localPosition);
                }
                else
                {
                    WallPlacement wallPlacement = WallPlacementFromOffsetVector(item, Vector3.back * i, Vector3.forward);

                    tempWalls[i] = wallPlacement;

                    placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(0, 0, -i - 1);//MakePositive(placeholderGameObjects[i].transform.localPosition);
                }


            }
        }
    }

}
