using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{

    //Child(0~3) : Refrigerator
    public GameObject storageUI;
    public List<Inventory[,]> allStorage;
    public Dictionary<char, int> totalInfo;

    public int storageWidth = 5;
    public int storageHeight = 6;

    void Awake()
    {
        totalInfo = new Dictionary<char, int>();
        allStorage = new List<Inventory[,]>();
    }

    void UpdateTotalInfo(Inventory[,] _storage)
    {
        int i, j, count;
        char ingredient;

        for (i = 0; i < storageHeight; i++)
        {
            for (j = 0; j < storageWidth; j++)
            {
                ingredient = _storage[j, i].ingredient;
                count = _storage[j, i].count;
                if (ingredient == '\0') { continue; }

                if (totalInfo.ContainsKey(ingredient))
                {
                    count += totalInfo[ingredient];
                    totalInfo.Remove(ingredient);
                    totalInfo.Add(ingredient, count);
                }
                else
                {
                    totalInfo.Add(ingredient, count);
                }
            }
        }
    }

    public void AddRefrigeratorStorage(Inventory[,] _storage)    //냉장고의 재료 정보 추가
    {
        allStorage.Add(_storage);
        UpdateTotalInfo(_storage);
        storageUI.GetComponent<StorageUI>().UpdateStorageInfo(allStorage);
    }

    public void UpdateRefrigeratorStorage(GameObject _child, Inventory[,] _storage)  //냉장고가 가진 재료 정보를 갱신
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _child)
            {
                allStorage[i] = _storage;
                UpdateTotalInfo(allStorage[i]);
            }
        }

        storageUI.GetComponent<StorageUI>().UpdateStorageInfo(allStorage);
    }

    public void ChangeViewRefrPages(GameObject _refr)  //냉장고 클릭 시에 페이지를 알맞게 변경할 수 있도록 함
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _refr)
            {
                storageUI.GetComponent<StorageUI>().SetViewRefrPageUI(i);
            }
        }
    }

    public int IndexOfRefrigeratorIndex(GameObject _refr)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetChild(0).gameObject == _refr) { return i; }
        }

        return -1;
    }

    public void DecreaseIngredient(MenuData.Menu menu)  //메뉴 제작 시 필요한 재료를 필요한 수만큼 감소시킴
    {
        Inventory inventory = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < menu.ingredients.Count; j++)
            {
                inventory = transform.GetChild(i).GetComponentInChildren<Refrigerator>().FindIngredientInStorage(menu.ingredients[j].sign);
                if (inventory != null)
                {
                    transform.GetChild(i).GetComponentInChildren<Refrigerator>().
                        DecreaseInventoryCount(inventory.horIndex, inventory.verIndex, menu.ingredients[j].require);
                }
            }
        }
    }

}
