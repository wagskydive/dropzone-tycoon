using System;
using System.Collections.Generic;
using DataLogic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryLogic
{

    public class ItemsLibrary
    {
        public Action<ItemsLibrary> OnLibraryModified;

        public List<ItemType> allItems = new List<ItemType>();



        public void AddNewItemType(string typeName)
        {
            if (allItems.Count > 0)
            {
                typeName = DataChecks.EnsureUnique(allItemTypeNames(), typeName);
            }

            ItemType itemType = new ItemType(typeName);
            allItems.Add(itemType);
            OnLibraryModified?.Invoke(this);
        }

        public void AddItemsFromStringArray(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {

                if (allItems.Count > 0)
                {
                    names[i] = DataChecks.EnsureUnique(allItemTypeNames(), names[i]);
                }

                ItemType itemType = new ItemType(names[i]);
                allItems.Add(itemType);
                
            }
            OnLibraryModified?.Invoke(this);
        }

        public void AddNewCraftingType(string crafterTypeName)
        {
            if (allItems.Count > 0)
            {
                crafterTypeName = DataChecks.EnsureUnique(allItemTypeNames(), crafterTypeName);
            }

            CrafterType crafterType = new CrafterType(crafterTypeName);
            allItems.Add(crafterType);
        }

        void MakeItemIntoCrafterType(int index)
        {
            CrafterType crafterType = new CrafterType(allItems[index].TypeName);
            allItems[index] = crafterType;
        }

        public void AddItemsToRecipe(ItemAmount itemAmount, int itemIndex)
        {
                        allItems[itemIndex].AddToRecipe(itemAmount);
            OnLibraryModified?.Invoke(this);
        }

        public int IndexFromTypeName(string typeName)
        {

            int index = allItems.FindIndex(x => x.TypeName == typeName);
            return index;
        }

        public void ModifyDescription(int index, string edit)
        {
            allItems[index].SetDescription(edit);
        }

        public string[] allItemTypeNames()
        {
            string[] names = new string[allItems.Count];
            for (int i = 0; i < allItems.Count; i++)
            {
                names[i] = allItems[i].TypeName;
            }
            return names;
        }

        public void RenameItemType(int index, string name)
        {
            allItems[index].SetTypeName(name);
        }

        public int GetMaxHiarchyLevelOfTree()
        {
            int max = 0;
            for (int i = 0; i < allItems.Count; i++)
            {
                int hiarchyLevel = GetHiarchyLevelOfItem(allItems[i].TypeName);
                if (hiarchyLevel > max)
                {
                    max = hiarchyLevel;
                }
            }
            return max;
        }

        public Recipe RecipeFromItemTypeName(string typeName)
        {
            return allItems[IndexFromTypeName(typeName)].recipe;
        }

        public int[] ItemTypesInRecipe(string typeName)
        {
            return ItemTypesInRecipe(RecipeFromItemTypeName(typeName));
        }

        public int[] ItemTypesInRecipe(Recipe recipe)
        {
            if(recipe!= null &&recipe.Ingredients!= null)
            {
                int[] output = new int[recipe.Ingredients.Length];
                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = IndexFromTypeName(recipe.Ingredients[i].itemType.TypeName);
                }
                return output;
            }
            else
            {
                return null;
            }
           
        }

        public int[] ValidIngredients(int index)
        {            
            List<int> validIngedients = new List<int>();
            int[] downstream = GetAllDownstreamItems(index);
            if (downstream == null)
            {
                for (int i = 0; i < allItems.Count; i++)
                {
                    if (i != index)
                    {
                        validIngedients.Add(i);
                    }
                }
            }
            else
            {           
                for (int i = 0; i < allItems.Count; i++)
                {
                    if (!downstream.Contains(i)&& i != index)
                    {
                        validIngedients.Add(i);
                    }
                }
            }
            return validIngedients.ToArray();
        }

        public int GetHiarchyLevelOfItem(string skillName)
        {
            int itemIndex = allItems.FindIndex(x => x.TypeName == skillName);
            return GetHiarchyLevelOfItem(itemIndex);
        }

        public int GetHiarchyLevelOfItem(int itemIndex)
        {
            int level = 0;
            ItemType itemToCheck = allItems[itemIndex];

            if (itemToCheck.recipe != null && itemToCheck.recipe.Ingredients.Length > 0)
            {
                int highestLevel = 1;
                for (int i = 0; i < itemToCheck.recipe.Ingredients.Length; i++)
                {

                    ItemAmount ingredient = itemToCheck.recipe.Ingredients[i];

                    int highestLevelOfIngredient = DataChecks.GetMax(GetHiarchyLevelOfItem(ingredient.itemType.TypeName) + 1, highestLevel);
                    highestLevel = DataChecks.GetMax(highestLevel, highestLevelOfIngredient);
                }
                level = DataChecks.GetMax(level, highestLevel);
            }
            return level;
        }

        public int[] GetAllDownstreamItems(int subject)
        {
            int[] itemsWithMeInRecipe = GetAllItemsThatHaveItemInRecipe(subject);
            if (itemsWithMeInRecipe == null)
            {
                return null;
            }


            List<int> downstreamList = new List<int>();

            downstreamList.AddRange(itemsWithMeInRecipe);

            for (int i = 0; i < itemsWithMeInRecipe.Length; i++)
            {
                int[] downStream = GetAllDownstreamItems(itemsWithMeInRecipe[i]);
                if (downStream != null && downStream.Length > 0)
                {
                    downstreamList.AddRange(downStream);
                }
            }
            return downstreamList.ToArray();

        }

        private int[] GetAllItemsThatHaveItemInRecipe(int subject)
        {
            ItemType itemType = allItems[subject];
            List<int> indeces = new List<int>();
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].recipe != null && allItems[i].recipe.Ingredients != null)
                {
                    for (int j = 0; j < allItems[i].recipe.Ingredients.Length; j++)
                    {
                        if (allItems[i].recipe.Ingredients[j].itemType.TypeName == itemType.TypeName)
                        {
                            indeces.Add(i);
                        }
                    }
                }

            }
            return indeces.ToArray();
        }

        //Skills that are requirements of subject recursive
        public int[] GetAllUpstreamItems(int subject)
        {
            if (allItems[subject].IsRoot())
            {
                return null;
            }
            List<int> upstreamList = new List<int>();
            if (allItems[subject].recipe != null && allItems[subject].recipe.Ingredients != null)
            {
                ItemAmount[] subjectIngedients = allItems[subject].recipe.Ingredients;
                
                for (int i = 0; i < subjectIngedients.Length; i++)
                {
                    upstreamList.Add(IndexFromTypeName(subjectIngedients[i].itemType.TypeName));
                }

                if (ReqsOfReqs(allItems[subject]) != null)
                {
                    for (int i = 0; i < subjectIngedients.Length; i++)
                    {
                        if (!subjectIngedients[i].itemType.IsRoot())
                        {
                            upstreamList.AddRange(GetAllUpstreamItems(IndexFromTypeName(subjectIngedients[i].itemType.TypeName)));
                        }
                    }
                }
            }
            return upstreamList.ToArray();
        }

        public int[] ReqsOfReqs(ItemType item)
        {
            List<int> reqList = new List<int>();
            if (item.recipe != null && item.recipe.Ingredients != null)
            {
                ItemAmount[] ingedients = item.recipe.Ingredients;

                if (ingedients != null)
                {
                    for (int i = 0; i < ingedients.Length; i++)
                    {
                        if (ingedients[i].itemType.recipe != null && ingedients[i].itemType.recipe.Ingredients != null)
                        {
                            ItemAmount[] recipeRecipe = ingedients[i].itemType.recipe.Ingredients;
                            if (recipeRecipe != null)
                            {
                                for (int j = 0; j < recipeRecipe.Length; j++)
                                {
                                    reqList.Add(IndexFromTypeName(recipeRecipe[j].itemType.TypeName));
                                }
                            }
                        }
                    }
                }
            }
            if (reqList.Count > 0)
            {
                return reqList.ToArray();
            }
            else
            {
                return null;
            }

        }

    }
}
