using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkydiveLogic;
using CharacterLogic;

public class DummySkydiveHelper : MonoBehaviour
{
    public AircraftObject aircraftObject;

    void Start()
    {
        aircraftObject.aircraft = new Aircraft();
        aircraftObject.aircraft.maxSlots = 5;
        aircraftObject.aircraft.typeName = "Dummy Cessna";


        Load fakeLoad = new Load();
        fakeLoad.maxSlots = aircraftObject.aircraft.maxSlots;

        string[] stats = new string[2];
        stats[0] = "AFF";
        stats[1] = "CP1";

        Slot slot1 = new Slot();
        slot1.character = new Character("Jumper1", stats);
        Slot slot2 = new Slot();
        slot1.character = new Character("Jumper2", stats);
        Slot slot3 = new Slot();
        slot1.character = new Character("Jumper3", stats);
        Slot slot4 = new Slot();
        slot1.character = new Character("Jumper4", stats);

        JumpGroup group1 = new JumpGroup();
        group1.ExitAltitude = 2000;
        group1.jumpType = JumpType.FS;
        group1.members = new List<Slot>();
        group1.members.Add(slot1);
        group1.members.Add(slot2);

        fakeLoad.AddGroupToLoad(group1);

        JumpGroup group2 = new JumpGroup();
        group2.ExitAltitude = 2000;
        group2.jumpType = JumpType.FF;
        group2.members = new List<Slot>();
        group2.members.Add(slot3);
        group2.members.Add(slot4);

        fakeLoad.AddGroupToLoad(group2);


        
        aircraftObject.aircraft.CurrentLoad = fakeLoad;

        aircraftObject.aircraft.SubscribeToCurrentLoad();
        aircraftObject.SubscribeToAircraft();
        Invoke("FakeLoadedFlag", 4);
    }
    void FakeLoadedFlag()
    {
        aircraftObject.aircraft.CurrentLoad.JumpersLoaded();
    }

}
