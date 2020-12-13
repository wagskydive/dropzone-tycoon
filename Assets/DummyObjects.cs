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
        return new Character("DummyCharacter_"+name, stats);        
    }

    public static ItemType ProvideDummeyItemType(string name = "")
    {
        ItemsLibrary library = new ItemsLibrary();

        library.AddNewItemType("DummyItemType" + name);
        library.AddNewItemType("Dummy Resource One");
        library.AddNewItemType("Dummy Resource Two");

        ItemAmount resourceOne = new ItemAmount(library.allItems[1], 10);
        library.AddItemsToRecipe(resourceOne, 0);
       

        ItemAmount resourceTwo = new ItemAmount(library.allItems[2], 20);
        library.AddItemsToRecipe(resourceTwo, 0);



        return library.allItems[0];
    }

    public static Item ProvideDummyItem()
    {
        ItemType itemType = ProvideDummeyItemType();
        Item item = new Item("/Items/DummyItem");
        return item;
    }
}

