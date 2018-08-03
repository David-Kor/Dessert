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

    private int count;
    private int remainingPeriod;
    private char ingredient;
    private const char NULL_CHAR = '\0';
    public GameObject expirationDateUI;
    
    void Start() {
        expirationDateUI = Instantiate(expirationDateUI, transform);
        expirationDateUI.SetActive(false);
    }

    public void ResetActiveExprirationDate()    //이 오브젝트의 유통기한 알림을 표시하지 않음
    {
        expirationDateUI.SetActive(false);
    }

    public static void ResetActiveAllExprirationDate()    //모든 오브젝트의 유통기한 알림을 표시하지 않음
    {
        GameObject storageUI = GameObject.Find("StorageUI");
        if (storageUI == null) { return; }


        bool preActive = false;
        IngredientUI[] ingredientUIs = null;
        for (int i = 1; i < storageUI.transform.childCount; i++)
        {
            preActive = storageUI.transform.GetChild(i).gameObject.activeSelf;
            storageUI.transform.GetChild(i).gameObject.SetActive(true);
            ingredientUIs = storageUI.transform.GetChild(i).GetComponentsInChildren<IngredientUI>();

            for (int j = 0; j < ingredientUIs.Length; j++)
            {
                ingredientUIs[j].ResetActiveExprirationDate();
            }
            storageUI.transform.GetChild(i).gameObject.SetActive(preActive);
        }
    }

    public void ViewExpirationDateOnClick()     //클릭하면 오직 자신만의 유통기한 알림 표시를 키거나 끔
    {
        if (!expirationDateUI.activeSelf)
        {
            ResetActiveAllExprirationDate();
            expirationDateUI.SetActive(true);
            expirationDateUI.transform.GetComponentInChildren<Text>().text = "잔여 유통기한 : " + remainingPeriod + " 일";
        }
        else { ResetActiveAllExprirationDate(); }
    }

    public void UpdateIndientInfo(Inventory _inventory)
    {
        ingredient = _inventory.ingredient;
        count = _inventory.count;
        remainingPeriod = _inventory.remainingPeriod;
        if (ingredient == NULL_CHAR)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Image>().sprite = _inventory.image;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetComponentInChildren<Text>().text = count.ToString();
        }

    }
    public char GetIngredient() { return ingredient; }
    public int GetRemainCount() { return count; }

}
