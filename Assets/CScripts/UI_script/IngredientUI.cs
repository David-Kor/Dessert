using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > StorageUI > RefrigeratorPageUI + RefrPageButtonUI > InventoryPanelUI > InventorySpaceUI > [ IngredientUI ] */

    //Child(0) : Image
    //Child(1) : The Number Of Remaining (img)
    //Child(1).Child(0) : The Number Of Remaining (Text)

    private int remain;
    private char ingredient;
    private const char NULL_CHAR = '\0';


    public void UpdateIndientInfo(Inventory _inventory)
    {
        ingredient = _inventory.ingredient;
        remain = _inventory.count;
        if (ingredient == NULL_CHAR)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetComponentInChildren<Text>().text = remain.ToString();
        }

    }

}
