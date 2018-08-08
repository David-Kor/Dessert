using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TITLE = EnumCollection.TITLE;

public class Refrigerator : MonoBehaviour
{
    public GameObject kitchenUI;
    private int storageWidth = 5;
    private int storageHeight = 6;
    public Inventory[,] storage; //냉장고 전체 보관 재료 (단위 : 페이지)
    public GameObject marker;
    private int myIndex;

    void Start()
    {
        int i, j;
        storageWidth = GetComponentInParent<Storage>().storageWidth;
        storageHeight = GetComponentInParent<Storage>().storageHeight;
        myIndex = GetComponentInParent<Storage>().IndexOfRefrigeratorIndex(gameObject);
        marker = transform.parent.GetChild(1).gameObject;
        storage = new Inventory[storageWidth, storageHeight];  //가로 5 x 세로 6 (칸)

        //storage 초기화
        List<Inventory> bufferInventory = SaveAndLoad.LoadInventory(myIndex);   //세이브 파일에서 Inventory정보를 읽어옴
        int hor, ver;
        for (i = 0; i < bufferInventory.Count; i++)
        {
            hor = bufferInventory[i].horIndex;
            ver = bufferInventory[i].verIndex;
            storage[hor, ver] = bufferInventory[i];
        }

        for (i = 0; i < storageHeight; i++)
        {
            for (j = 0; j < storageWidth; j++)
            {
                //채워지지 않은 칸은 초기화
                if (storage[j, i] == null)
                {
                    storage[j, i] = new Inventory('\0', 0, 0, j, i);
                }
            }
        }

        GetComponentInParent<Storage>().AddRefrigeratorStorage(transform.parent.gameObject, storage);

    }


    void Update()
    {

        if (GetComponent<ObjectEventManager>().doCancel)    //선택 취소
        {
            GetComponent<ObjectEventManager>().doCancel = false;
            marker.GetComponent<SpriteRenderer>().enabled = false;
            IngredientUI.ResetActiveAllExprirationDate();   //재료 유통기한 표시 비활성화
        }

        if (GetComponent<ObjectEventManager>().doEvent) //클릭 이벤트 발생
        {
            ObjectEventManager.CancelAllSelectWithOutObject(transform.parent.gameObject); //다른 오브젝트 선택 해제
            Camera.main.GetComponent<EventListener>().ResetSelectPlayer();  //플레이어 선택 해제
            GetComponent<ObjectEventManager>().doEvent = false;
            //Marker 활성화/비활성화
            marker.GetComponent<SpriteRenderer>().enabled = !marker.GetComponent<SpriteRenderer>().enabled;

            IngredientUI.ResetActiveAllExprirationDate();   //재료 유통기한 표시 비활성화
            //Marker 활성화 시 UI도 활성화
            if (marker.GetComponent<SpriteRenderer>().enabled)
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.STORAGE);   //Marker 활성화 시 주방의 UI를 Cook로 전환
                GetComponentInParent<Storage>().ChangeViewRefrPages(transform.parent.gameObject);
            }
            //Marker 비활성화 시 UI도 비활성화
            else
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.NONE);   //Marker 비활성화 시 주방의 UI를 초기화
            }

        }

    }

    public Inventory FindIngredientInStorage(char _ingredient)  //재료가 냉장고안에 존재하는지 확인
    {
        for (int i = 0; i < storageHeight; i++)
        {
            for (int j = 0; j < storageWidth; j++)
            {
                if (storage[j, i].ingredient == _ingredient) { return storage[j, i]; }
            }
        }
        return null;
    }

    public List<Inventory> FindIngredientsInStorage(char _ingredient)  //재료가 냉장고안에 존재하는지 확인
    {
        List<Inventory> findInventory = new List<Inventory>();

        for (int i = 0; i < storageHeight; i++)
        {
            for (int j = 0; j < storageWidth; j++)
            {
                if (_ingredient == storage[j, i].ingredient)
                {
                    findInventory.Add(storage[j, i]);
                }
            }
        }

        return findInventory;
    }

    public void DecreaseInventoryCount(int _hor, int _ver, int decrsValue)
    {
        storage[_hor, _ver].count -= decrsValue;    //요구 개수만큼 차감
        if (storage[_hor, _ver].count <= 0)
        {
            storage[_hor, _ver].ingredient = '\0';
            storage[_hor, _ver].remainingPeriod = 0;
            storage[_hor, _ver].UpdateImageByIngr();
        }

        GetComponentInParent<Storage>().UpdateRefrigeratorStorage(transform.parent.gameObject, storage);
    }
}
