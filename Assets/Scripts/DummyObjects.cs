using System;
using UnityEngine;
using InventoryLogic;
using CharacterLogic;


public static class DummyObjects
{
    public static Character ProvideDummyCharacter(string name = "")
    {
        string[] stats = new string[2];
        stats[0] = "Health";
        stats[1] = "isDummy";
        Character dummy = new Character("DummyCharacter_" + name, stats);
        
        return dummy;
    }

    public static ItemType ProvideDummeyItemType(string name = "")
    {
        ItemsLibrary library = new ItemsLibrary("DummyLibrary");

        library.AddNewItemType(name, "Items/");
        library.AddNewItemType("Dummy Resource One", "Items/");
        library.AddNewItemType("Dummy Resource Two", "Items/");

        ItemAmount resourceOne = new ItemAmount(library.allItems[1], 10);
        library.AddItemsToRecipe(resourceOne, 0);


        ItemAmount resourceTwo = new ItemAmount(library.allItems[2], 20);
        library.AddItemsToRecipe(resourceTwo, 0);



        return library.allItems[0];
    }

    public static Item ProvideDummyItem()
    {
        ItemType itemType = ProvideDummeyItemType("DummyItem");
        Item item = new Item(itemType);
        return item;
    }
}

