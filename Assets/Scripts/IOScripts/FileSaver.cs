using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SkillsLogic;
using DataLogic;
using System.Text;
using InventoryLogic;
using System;

using System.Linq;

public class FileSaver
{



    public static string SkillTreeToJson(string path, Skill[] tree)
    {
        JSONObject treeObject = new JSONObject();




        for (int i = 0; i < tree.Length; i++)
        {
            JSONObject skillObject = new JSONObject();
            skillObject.Add("Name", tree[i].Name);
            skillObject.Add("Description", tree[i].Description);


            if (tree[i].RequiredSkills != null)
            {
                JSONArray requirementsArray = new JSONArray();

                for (int j = 0; j < tree[i].RequiredSkills.Length; j++)
                {
                    requirementsArray.Add("Req " + j.ToString(), new JSONString(tree[tree[i].RequiredSkills[j]].Name));
                }
                Debug.Log(requirementsArray.ToString());

                skillObject.Add("Requirements", requirementsArray);
            }


            if (tree[i].Effectors != null)
            {
                JSONObject effectors = new JSONObject();

                foreach (KeyValuePair<string, float> effector in tree[i].Effectors)
                {
                    effectors.Add(effector.Key, System.Math.Round(effector.Value, 3));


                }
                skillObject.Add("Effectors", effectors);
            }

            treeObject.Add(tree[i].Name, skillObject);
        }


        Debug.Log(treeObject.ToString());
        File.WriteAllText(path, treeObject.ToString());
        return treeObject.ToString();

    }

    internal static string WriteLibraryToJson(string path, ItemsLibrary library)
    {
        List<ItemType> allItems = library.allItems;
        JSONObject treeObject = new JSONObject();



        for (int i = 0; i < allItems.Count; i++)
        {
            JSONObject itemObject = ItemTypeToJson(allItems[i]);

            treeObject.Add(allItems[i].TypeName, itemObject);
        }


        Debug.Log(treeObject.ToString());
        File.WriteAllText(path, treeObject.ToString());
        return treeObject.ToString();
    }



    internal static ItemType JsonToItemTypeNoIngredients(JSONObject jsonObject)
    {
        JSONNode node = new JSONObject();
        //node = 
        string nameString = jsonObject[0].Value;
        string resourcePath = jsonObject[1].Value;
        string catagoryString = jsonObject[2].Value;
        string descriptionString = jsonObject[3].Value;



        ItemType itemType = new ItemType(nameString, resourcePath, catagoryString, descriptionString);

        return itemType;
    }

    internal static ItemsLibrary JsonToItemLibrary(string path, string libraryName)
    {
        ItemsLibrary library = new ItemsLibrary(libraryName);

        JSONObject itemsRead = JSONNode.Parse(File.ReadAllText(path)).AsObject;



        //Create ItemTypes
        for (int i = 0; i < itemsRead.Count; i++)
        {
            JSONObject itemObject = itemsRead[i].AsObject;
            ItemType type = JsonToItemTypeNoIngredients(itemObject);

            library.AddNewItemType(type);
        }

        //Set Recipe References
        for (int i = 0; i < itemsRead.Count; i++)
        {
            AddRecipeReference(library, itemsRead, i);
        }


        return library;
    }

    private static void AddRecipeReference(ItemsLibrary library, JSONObject itemsRead, int i)
    {
        JSONObject itemObject = itemsRead[i].AsObject;
        if (itemObject.HasKey("Ingredients"))
        {
            JSONObject ingredientObject = itemObject.GetValueOrDefault("Ingredients", itemObject).AsObject;
            for (int j = 0; j < ingredientObject.Count; j++)
            {
                int cur = library.IndexFromTypeName(ingredientObject[j].Keys.Current);
                int amount = ingredientObject[j].GetValueOrDefault(ingredientObject[j].Keys.Current, ingredientObject[j]).AsInt;

                ItemAmount itemAmount = new ItemAmount(library.allItems[cur], amount);
                library.AddItemsToRecipe(itemAmount, i);

                //ItemAmount itemAmount = new ItemAmount()
            }
            int outputAmount = itemObject.GetValueOrDefault("OutputAmount", itemObject).AsInt;

            library.SetOutputAmount(outputAmount, i);
        }
    }

    public static Skill[] JsonToSkillTree(string path)
    {
        JSONObject treeRead = JSONNode.Parse(File.ReadAllText(path)).AsObject;


        Skill[] skills = new Skill[treeRead.Count];
        // Initialize skill tree
        for (int i = 0; i < treeRead.Count; i++)
        {
            JSONObject skillObject = treeRead[i].AsObject;

            Skill skill = new Skill(skillObject.GetValueOrDefault("Name", skillObject));
            skill.SetDescription(skillObject.GetValueOrDefault("Description", skillObject));



            JSONObject effectors = skillObject.GetValueOrDefault("Effectors", skillObject).AsObject;
            if (effectors != null)
            {
                Dictionary<string, float> Eff = new Dictionary<string, float>();

                foreach (KeyValuePair<string, JSONNode> eff in effectors)
                {
                    Eff.Add(eff.Key, eff.Value);
                }
                skill.SetEffectors(Eff);
            }

            skills[i] = skill;
            Debug.Log("Found object: " + treeRead.Keys.Current + " " + skillObject.ToString());
        }

        // Set Requirements
        for (int i = 0; i < treeRead.Count; i++)
        {
            JSONArray req = treeRead[i].AsObject.GetValueOrDefault("Requirements", treeRead[i].AsObject).AsArray;
            if (req != null)
            {
                int[] reqString = new int[req.Count];
                for (int j = 0; j < req.Count; j++)
                {
                    reqString[j] = SkillTreeDataHandler.FindIndexOfSkillByNameInSkillArray(skills, req[j].Value);
                }
                skills[i].SetRequieredSkills(reqString);
            }

        }
        return skills;

    }

    public static void RenameFile(string oldName, string newName)
    {
        if (oldName != newName)
        {
            //if(File.Exi)
            File.Copy(oldName, newName);
            //File.Delete(oldName);
        }
    }

    public static string FormatName(string name)
    {
        string str = string.Concat(name.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');



        if (str.Length == 0)
            return "Empty String";
        else if (str.Length == 1)
            return str.ToUpper();
        else
            return char.ToUpper(str[0]) + str.Substring(1);
    }

    public static void RenameFormatted(string original)
    {
        RenameFile(original, FormatName(original));
    }

    public static void RenameFilesWithExtentionInFolder(string path, string extention)
    {
        var info = new DirectoryInfo(path);
        FileInfo[] fileInfos = info.GetFiles();
        List<string> files = new List<string>();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string extentionFound = Path.GetExtension(fileInfos[i].Name);
            if (extentionFound == "." + extention)
            {
                string newName = FormatName(fileInfos[i].Name);
                RenameFile(path + fileInfos[i].Name, path + "Renamed/" + newName);
                //string result = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - extentionFound.Length);
                //files.Add(result);
            }
        }
    }

    public static JSONObject ItemTypeToJson(ItemType itemType)
    {
        JSONObject itemObject = new JSONObject();
        itemObject.Add("Name", itemType.TypeName);
        itemObject.Add("ResourcePath", itemType.ResourcePath);
        itemObject.Add("Catagory", itemType.Catagory);

        itemObject.Add("Description", itemType.Description);


        if (!itemType.IsRoot())
        {
            JSONObject ingredientsObject = new JSONObject();
            ItemAmount[] ingredients = itemType.recipe.Ingredients;


            for (int j = 0; j < ingredients.Length; j++)
            {
                JSONObject ingredient = new JSONObject();
                ingredient.Add(ingredients[j].itemType.TypeName, ingredients[j].Amount);
                ingredientsObject.Add(ingredient);
            }
            Debug.Log(ingredientsObject.ToString());

            itemObject.Add("Ingredients", ingredientsObject);
            itemObject.Add("OutputAmount", new JSONNumber(itemType.recipe.OutputAmount));
        }

        return itemObject;
    }


    public static Structure JsonToStructureBluePrint(string path, ItemsLibrary itemsLibrary)
    {
        JSONObject structureRead = JSONNode.Parse(File.ReadAllText(path)).AsObject;

        string nameString = structureRead.GetValueOrDefault("Name", structureRead);
        float gridSize = structureRead.GetValueOrDefault("GridSize", structureRead);
        float floorSize = structureRead.GetValueOrDefault("FloorSize", structureRead);

        Structure structure = new Structure(nameString, gridSize, floorSize);
        JSONArray walls = structureRead.GetValueOrDefault("Walls", structureRead).AsArray;
        for (int i = 0; i < walls.Count; i++)
        {
            structure.AddPartAsWall(JsonPlacementToWallPlacement(walls[i].AsObject, itemsLibrary));
        }

        JSONArray floors = structureRead.GetValueOrDefault("Floors", structureRead).AsArray;
        for (int i = 0; i < floors.Count; i++)
        {
            structure.AddPartAsFloor(JsonToFloorPlacement(floors[i].AsObject, itemsLibrary));
        }

        return structure;
    }


    public static void SaveStructureBluePrint(string path, Structure structure)
    {
        JSONObject structureObject = StructureBlueprintToJson(structure);
        File.WriteAllText(path, structureObject.ToString());
        
    }



    public static JSONObject StructureBlueprintToJson(Structure structure)
    {
        JSONObject structureObject = new JSONObject();
        structureObject.Add("Name", structure.Name);
        structureObject.Add("GridSize", structure.GridSize);
        structureObject.Add("FloorSize", structure.FloorSize);
        JSONArray walls = new JSONArray();
        for (int i = 0; i < structure.walls.Count; i++)
        {
            JSONObject wall = WallPlacementToJson(structure.walls[i]).AsObject;
            walls.Add(wall);
        }
        structureObject.Add("Walls", walls);

        JSONArray floors = new JSONArray();
        for (int i = 0; i < structure.floors.Count; i++)
        {
            floors.Add(FloorPlacementToJson(structure.floors[i]).AsObject);
        }
        structureObject.Add("Floors", floors);

        return structureObject;
    }

    public static JSONObject WallPlacementToJson(WallPlacement wallPlacement)
    {
        JSONObject wallObject = new JSONObject();
        wallObject.Add("StartPoint", GridPositionToJson(wallPlacement.StartPoint));
        wallObject.Add("EndPoint", GridPositionToJson(wallPlacement.EndPoint));
        wallObject.Add("Floor", wallPlacement.Floor);
        wallObject.Add("Item", wallPlacement.item.itemType.TypeName);
        wallObject.Add("Stretch", wallPlacement.Stretch);
        return wallObject;
    }

    public static WallPlacement JsonPlacementToWallPlacement(JSONObject wallJson, ItemsLibrary itemsLibrary)
    {

        JSONObject startPointObject = wallJson.GetValueOrDefault("StartPoint", wallJson).AsObject;
        GridPosition startPoint = JsonToGridPosition(startPointObject);

        JSONObject endPointObject = wallJson.GetValueOrDefault("EndPoint", wallJson).AsObject;
        GridPosition endPoint = JsonToGridPosition(endPointObject);

        int floor = wallJson.GetValueOrDefault("Floor", wallJson);

        ItemType itemType = itemsLibrary.allItems[itemsLibrary.IndexFromTypeName(wallJson.GetValueOrDefault("Item", wallJson))];
        bool stretch = wallJson.GetValueOrDefault("Stretch", wallJson);

        WallPlacement wallObject = new WallPlacement(new Item(itemType), startPoint, endPoint, floor, stretch);

        return wallObject;
    }

    public static JSONObject FloorPlacementToJson(FloorPlacement floorPlacement)
    {
        JSONObject floorObject = new JSONObject();
        floorObject.Add("GridPostion", GridPositionToJson(floorPlacement.gridPosition));
        floorObject.Add("Floor", floorPlacement.Floor);
        floorObject.Add("Item", floorPlacement.item.itemType.TypeName);

        return floorObject;
    }

    public static FloorPlacement JsonToFloorPlacement(JSONObject floorPlacement, ItemsLibrary itemsLibrary)
    {
        JSONObject gridPointObject = floorPlacement.GetValueOrDefault("StartPoint", floorPlacement).AsObject;
        GridPosition gridPoint = JsonToGridPosition(gridPointObject);

        int floor = floorPlacement.GetValueOrDefault("Floor", floorPlacement);

        ItemType itemType = itemsLibrary.allItems[itemsLibrary.IndexFromTypeName(floorPlacement.GetValueOrDefault("Item", floorPlacement))];

        FloorPlacement floorObject = new FloorPlacement(new Item(itemType), gridPoint, floor);


        return floorObject;
    }

    public static JSONObject GridPositionToJson(GridPosition gridPosition)
    {
        JSONObject gridObject = new JSONObject();
        gridObject.Add("X", gridPosition.X);
        gridObject.Add("Y", gridPosition.Y);
        return gridObject;
    }

    public static GridPosition JsonToGridPosition(JSONObject gridPosition)
    {

        int X = gridPosition.GetValueOrDefault("X", gridPosition);
        int Y = gridPosition.GetValueOrDefault("Y", gridPosition);

        return new GridPosition(X, Y);
    }

}

