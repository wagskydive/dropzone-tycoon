using InventoryLogic;
using SpawnLogic;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WallBoxPlacer : ItemPlacer
{
    public StructureObject workingStructure { get; private set; }
    int currentFloor;

    GridPosition startPoint;
    int[] lastDistancesForBox = { 0, 0 };


    public override event Action OnItemPlaced;
    public event Action<WallPlacement> OnWallPlaced;
    
    public override event Action<ISpawnable, Transform, Transform> OnSpawnRequest;

    List<WallPlacement> tempWalls = new List<WallPlacement>();

    public override void Awake()
    {
        base.Awake();
        if (workingStructure != null)
        {
            workingStructure.AssignSpawner(spawner);
        }
    }

    public void SetWorkingStructure(ItemSpawner itemSpawner, StructureObject structure, int floor)
    {
        currentFloor = floor;
        workingStructure = structure;
        workingStructure.AssignSpawner(itemSpawner);
    }

    private void AdjustListLenghtForBox(int[] distance)
    {
        int visablePlaceholders = (Math.Abs(distance[0]) + Math.Abs(distance[1])) * 2;
        int placeCount = placeholderGameObjects.Count;
        if (placeCount < visablePlaceholders)
        {

            for (int i = 0; i < visablePlaceholders - placeCount; i++)
            {
                //GridPosition gP = new GridPosition(startPoint.X + distance[0] - 1, startPoint.Y);
                GameObject go = InstantiatePlaceHolder(currentSpawnable, workingStructure.transform, Vector3.zero);

                placeholderGameObjects.Add(go);
                tempWalls.Add(new WallPlacement((Item)currentSpawnable, new GridPosition(0, 0), new GridPosition(0, 0), currentFloor));
            }


        }
    }

    void HandleBoxPlacement(Vector3 pointerPos)
    {
        int[] distances = startPoint.GridDistance(Vector3ToGridPosition(pointerPos-workingStructure.transform.position));

        Debug.Log("Distance X: " + distances[0] + "\n" + "Distance Y: " + distances[1]);

        if (lastDistancesForBox[0] != distances[0] || lastDistancesForBox[1] != distances[1])
        {
            AdjustListLenghtForBox(distances);
            HandlePlaceHolderBoxPositioning(distances);
            HandlePlaceHolderActivations(Math.Abs(distances[0] * 2) + Math.Abs(distances[1] * 2));
        }
        lastDistancesForBox = distances;
    }

    public override void SetFirstPlacementObject(ISpawnable objectToAdd, bool snap = false, float grSize = 1)
    {
        base.SetFirstPlacementObject(objectToAdd, snap, grSize);
        GameObject go = InstantiatePlaceHolder(objectToAdd, gameObject.transform);
        DestroyListComplete();
        placeholderGameObjects.Add(go);
        MouseDetect.OnLeftClickDetected += FirstClick;
    }

    public override void FirstClick(Vector3 pos)
    {
        base.FirstClick(pos);
        if (workingStructure.transform.childCount == 0)
        {
            workingStructure.transform.position = pos;
            startPoint = Vector3ToGridPosition(Vector3.zero);

        }
        else
        {
            startPoint = Vector3ToGridPosition(pos - workingStructure.transform.position);
        }

        placeholderGameObjects[0].transform.SetParent(workingStructure.transform);


            //placeholderGameObjects[0].transform.position = pos;
        MouseDetect.OnLeftClickDetected += ConfirmPlacementRequest;
        MouseDetect.OnRightClickDetected += CancelPlacement;
        MouseDetect.OnOverDetected += HandleBoxPlacement;
    }

    public override void CancelPlacement(Vector3 pointerPos)
    {
        base.CancelPlacement(pointerPos);
        MouseDetect.OnLeftClickDetected -= ConfirmPlacementRequest;
        MouseDetect.OnRightClickDetected -= CancelPlacement;
        MouseDetect.OnOverDetected -= HandleBoxPlacement;
    }

    public override void ConfirmPlacementRequest(Vector3 pointerPos)
    {
        MouseDetect.OnLeftClickDetected -= ConfirmPlacementRequest;
        MouseDetect.OnOverDetected -= HandleBoxPlacement;
        int amount = Math.Abs(lastDistancesForBox[0] * 2) + Math.Abs(lastDistancesForBox[1] * 2);
        for (int i = 0; i < amount; i++)
        {
            placeholderGameObjects[i].SetActive(false);

            OnSpawnRequest?.Invoke(currentSpawnable, placeholderGameObjects[i].transform, workingStructure.transform);

            OnWallPlaced?.Invoke(tempWalls[i]);
            OnItemPlaced?.Invoke();
        }

        DestroyListComplete();

        gameObject.SetActive(false);



        base.ConfirmPlacementRequest(pointerPos);
    }


    private void HandlePlaceHolderBoxPositioning(int[] distances)
    {
        //TODO: Works only positive now...
        for (int i = 0; i < Math.Abs(distances[0] * 2) + Math.Abs(distances[1] * 2); i++)
        {
            int xOffset = i;
            if (distances[0] < 0)
            {
                xOffset *= -1;
            }

            int yOffset = i;
            if (distances[1] < 0)
            {
                yOffset *= -1;
            }
            if (i < Math.Abs(distances[0]))
            {
                Vector3 OffsetVector = new Vector3(xOffset, 0, 0);

                WallPlacement wallPlacement = WallPlacementFromOffsetVector(OffsetVector, Vector3.right);

                tempWalls[i] = wallPlacement;

                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(xOffset, 0, 0);
                placeholderGameObjects[i].transform.localRotation = Quaternion.identity;
            }
            else if (i < Math.Abs(distances[0] * 2))
            {
                xOffset -= Math.Abs(distances[0]);
                Vector3 OffsetVector = new Vector3(xOffset, 0, distances[1]);
                WallPlacement wallPlacement = WallPlacementFromOffsetVector(OffsetVector, Vector3.right);

                tempWalls[i] = wallPlacement;

                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(xOffset, 0, distances[1]);
                placeholderGameObjects[i].transform.localRotation = Quaternion.identity;
            }
            else if (i < Math.Abs(distances[1]) + Math.Abs(distances[0] * 2))
            {
                yOffset -= Math.Abs(distances[0] * 2);
                Vector3 OffsetVector = new Vector3(0, 0, yOffset);
                WallPlacement wallPlacement = WallPlacementFromOffsetVector(OffsetVector, Vector3.forward);

                tempWalls[i] = wallPlacement;


                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(0, 0, yOffset);
                placeholderGameObjects[i].transform.localRotation = Quaternion.identity;
                placeholderGameObjects[i].transform.Rotate(Vector3.up, -90);
            }
            else
            {
                yOffset -= Math.Abs(distances[0] * 2) + Math.Abs(distances[1]);

                Vector3 OffsetVector = new Vector3(distances[0], 0, yOffset);
                WallPlacement wallPlacement = WallPlacementFromOffsetVector(OffsetVector, Vector3.forward);

                tempWalls[i] = wallPlacement;


                placeholderGameObjects[i].transform.localPosition = GridPositionToVector3(startPoint) + new Vector3(distances[0], 0, yOffset);
                placeholderGameObjects[i].transform.localRotation = Quaternion.identity;
                placeholderGameObjects[i].transform.Rotate(Vector3.up, -90);
            }
        }
    }

    private WallPlacement WallPlacementFromOffsetVector(Vector3 OffsetVector, Vector3 direction)
    {
        Vector3 norm = MakePositive(OffsetVector.normalized);
        
        GridPosition localStartPoint = Vector3ToGridPosition(GridPositionToVector3(startPoint) + OffsetVector);
        GridPosition localEndPoint = Vector3ToGridPosition(GridPositionToVector3(startPoint) + OffsetVector + direction);
        WallPlacement wallPlacement = new WallPlacement((Item)currentSpawnable, localStartPoint, localEndPoint, currentFloor);
        return wallPlacement;
    }
}
