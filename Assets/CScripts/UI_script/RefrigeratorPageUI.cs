using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorPageUI : MonoBehaviour {
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > StorageUI > [ RefrigeratorPageUI ] + RefrPageButtonUI > InventoryPanelUI > InventorySpaceUI > IngredientUI */

    //Child(0) : InventoryPanelUI

    private Inventory[,] myStorage;
    private GameObject inventoryPanelUI;
    private GameObject[,] inventorySpaceUI;
    private int storageWidth;
    private int storageHeight;

    void Awake()
    {
        inventoryPanelUI = transform.GetChild(0).gameObject;
        storageWidth = GameObject.Find("Storage").GetComponent<Storage>().storageWidth;
        storageHeight = GameObject.Find("Storage").GetComponent<Storage>().storageHeight;
    }

    public void SetStorage(Inventory[,] _storage)
    {
        int counter = 0;
        myStorage = _storage;
        for (int i = 0; i < storageHeight; i++)
        {
            for (int j = 0; j < storageWidth; j++)
            {
                inventoryPanelUI.transform.GetChild(counter++)
                    .GetComponent<InventorySpaceUI>().SetInventory(myStorage[j, i]);
            }
        }
    }
}
