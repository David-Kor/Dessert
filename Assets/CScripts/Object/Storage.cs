using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public GameObject storageUI;
    public List<Inventory[,]> storageInfo;
    public int storageWidth = 5;
    public int storageHeight = 6;

    void Awake()
    {
        storageInfo = new List<Inventory[,]>();
    }

    public void AddRefrigeratorStorage(Inventory[,] _storage)    //냉장고의 재료 정보 추가
    {
        storageInfo.Add(_storage);
        storageUI.GetComponent<StorageUI>().UpdateStorageInfo(storageInfo);
    }

    public void UpdateRefrigeratorStorage(GameObject _child, Inventory[,] _storage)  //냉장고가 가진 재료 정보를 갱신
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _child)
            {
                storageInfo[i] = _storage;
            }
        }
        storageUI.GetComponent<StorageUI>().UpdateStorageInfo(storageInfo);
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
}
