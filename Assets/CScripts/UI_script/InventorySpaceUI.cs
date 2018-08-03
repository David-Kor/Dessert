using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpaceUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > StorageUI > RefrigeratorPageUI + RefrPageButtonUI > InventoryPanelUI > [ InventorySpaceUI ] > IngredientUI */

    //Child(0) : Ingredient(EmptyObject(parent) -> Image+Text (childs))
    //Child(1) : ExprationDate(Text)
    public GameObject IngredientUI;
    private Inventory inventory;

    void Awake()
    {
        IngredientUI = Instantiate(IngredientUI, transform);
    }

    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
        GetComponentInChildren<IngredientUI>().UpdateIndientInfo(inventory);
    }
    
    public Inventory GetInventory() { return inventory; }

}
