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
    public GameObject expirationDateUI;
    private Inventory inventory;

    void Awake()
    {
        IngredientUI = Instantiate(IngredientUI, transform);
        expirationDateUI = Instantiate(expirationDateUI, transform);
        expirationDateUI.SetActive(false);
    }

    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
        GetComponentInChildren<IngredientUI>().UpdateIndientInfo(inventory);
    }

    public void ResetActiveExprirationDate()    //이 오브젝트의 유통기한 알림을 표시하지 않음
    {
        expirationDateUI.SetActive(false);
    }

    public void ResetActiveAllExprirationDate()    //모든 오브젝트의 유통기한 알림을 표시하지 않음
    {
        GameObject inventoryPanel = transform.parent.gameObject;
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            inventoryPanel.transform.GetChild(i).GetComponent<InventorySpaceUI>().ResetActiveExprirationDate();
        }
    }

    public void ViewExpirationDateOnClick()     //클릭하면 오직 자신만의 유통기한 알림 표시를 키거나 끔
    {
        if (!expirationDateUI.activeSelf)
        {
            ResetActiveAllExprirationDate();
            expirationDateUI.SetActive(true);
            expirationDateUI.transform.GetComponentInChildren<Text>().text = "잔여 유통기한 : " + inventory.remainingPeriod + " 일";
        }
        else { ResetActiveAllExprirationDate(); }
    }

    public Inventory GetInventory() { return inventory; }

}
