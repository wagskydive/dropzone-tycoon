using InventoryLogic;
using UnityEngine;

public interface ITreeBrowser
{

    void RefreshTree(ItemsLibrary library);


    void SelectNode(ItemNodeRuntimeOld node);


    void ResetAllColors();

    GameObject InstantiateNodeObject(string inputText);
}