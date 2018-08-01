using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StorageSpace = Refrigerator.StorageSpace;

public class Storage : MonoBehaviour
{
    public GameObject storageUI;
    public List<StorageSpace[,]> refrigeratorStorages;

    void Awake()
    {
        refrigeratorStorages = new List<StorageSpace[,]>();
    }

    public void AddRefrigeratorStorage(StorageSpace[,] _storage)    //냉장고의 재료 정보 추가
    {
        refrigeratorStorages.Add(_storage);
    }

    public void UpdateRefrigeratorStorage(GameObject _child, StorageSpace[,] _storage)  //냉장고가 가진 재료 정보를 갱신
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _child)
            {
                refrigeratorStorages[i] = _storage;
            }
        }
    }

    public void ChangeViewRefrPages(GameObject _refr)  //냉장고 클릭 시에 페이지를 알맞게 변경할 수 있도록 함
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == _refr)
            {
            }
        }
    }
}
