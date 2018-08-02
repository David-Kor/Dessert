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
                    storage[j, i] = new Inventory
                    {
                        ingredient = '\0',
                        remainingPeriod = 0,
                        count = 0,
                        horIndex = j,
                        verIndex = i
                    };
                }
            }
        }

        GetComponentInParent<Storage>().AddRefrigeratorStorage(storage);

    }


    void Update()
    {

        if (GetComponent<ObjectEventManager>().doCancel)    //선택 취소
        {
            GetComponent<ObjectEventManager>().doCancel = false;
            marker.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (GetComponent<ObjectEventManager>().doEvent) //클릭 이벤트 발생
        {

            ObjectEventManager.CancelAllSelectWithOutObject(transform.parent.gameObject); //다른 오브젝트 선택 해제
            Camera.main.GetComponent<EventListener>().ResetSelectPlayer();  //플레이어 선택 해제
            GetComponent<ObjectEventManager>().doEvent = false;
            //Marker 활성화/비활성화
            marker.GetComponent<SpriteRenderer>().enabled = !marker.GetComponent<SpriteRenderer>().enabled;

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

}
