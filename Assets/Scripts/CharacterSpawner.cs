using System;
using CharacterLogic;
using SpawnLogic;
using UnityEngine;

public class CharacterSpawner : Spawner
{
    public GameObject characterPrefab;

    public void SpawnCharacter(Character character, Transform position)
    {
        LastSpawn.AddComponent<CharacterObject>().character = character;
    }

    public override void Spawn(ISpawnable spawnable, Transform position, Transform parent)
    {
        base.Spawn(spawnable, position, parent);
        SpawnCharacter((Character)spawnable, position);
    }
}
