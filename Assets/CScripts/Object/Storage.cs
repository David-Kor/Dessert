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
        List<Inventory> inventory = null;
        int[] total = new int[menu.ingredients.Count];
        for (int i = 0; i < menu.ingredients.Count; i++)
        {
            total[i] = menu.ingredients[i].require;
        }

        for (int i = 0; i < transform.childCount; i++)  //냉장고 단위 반복
        {
            for (int j = 0; j < menu.ingredients.Count; j++)    //재료 단위 반복
            {
                //[i]번째 냉장고에서 [j]번째 재료가 들어있는 Inventory(칸)을 전부 받아옴
                inventory = transform.GetChild(i).GetComponentInChildren<Refrigerator>().FindIngredientsInStorage(menu.ingredients[j].sign);

                for (int k = 0; k < inventory.Count; k++)
                {
                    //[i]번째 냉장고에서 [j]번째 재료의 수가 충족 될 때까지 [k]번째 인벤토리를 차감해나감
                    if (total[j] > 0)
                    {
                        if (total[j] <= inventory[k].count) //[k]번째 인벤토리가 가진 개수가 충분하면 필요 개수만큼 전부 차감
                        {
                            transform.GetChild(i).GetComponentInChildren<Refrigerator>().
                               DecreaseInventoryCount(inventory[k].horIndex, inventory[k].verIndex, total[j]);
                            total[j] = 0;
                        }
                        else    //[k]번째 인벤토리가 가진 개수가 부족하면 남아있는 재료 전부 차감
                        {
                            transform.GetChild(i).GetComponentInChildren<Refrigerator>().
                               DecreaseInventoryCount(inventory[k].horIndex, inventory[k].verIndex, inventory[k].count);
                            total[j] -= inventory[k].count;     //차감된 만큼 필요 개수 감소
                        }

                    }

                }

            }

        }


    }



}
