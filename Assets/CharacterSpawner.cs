using System;
using CharacterLogic;
using SpawnLogic;
using UnityEngine;

public class CharacterSpawner : Spawner
{
    public GameObject characterPrefab;

    public void SpawnCharacter(Character character, Vector3 position)
    {
        LastSpawn.AddComponent<CharacterObject>().character = character;
    }

    public override void Spawn(ISpawnable spawnable, Vector3 position, Transform parent)
    {
        base.Spawn(spawnable, position, parent);
        SpawnCharacter((Character)spawnable, position);
    }
}
