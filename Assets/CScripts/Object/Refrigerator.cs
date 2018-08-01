using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TITLE = EnumCollection.TITLE;

public class Refrigerator : MonoBehaviour
{
    public struct StorageSpace  //냉장고 저장 공간 (단위 : 칸)
    {
        public int spaceID;
        public char ingredient;    //재료 기호
        public int remainPeriod;   //유통기한까지 남은 시간(단위 : 일)
        public int groceriesLeft;  //남은 개수
    }

    public GameObject kitchenUI;
    public int storageWidth = 5;
    public int storageHeight = 6;
    public StorageSpace[,] storage; //냉장고 전체 재료 (단위 : 페이지)
    public GameObject marker;

    void Start()
    {
        marker = transform.parent.GetChild(1).gameObject;
        storage = new StorageSpace[storageWidth, storageHeight];  //가로 5 x 세로 6 (칸)
        //초기화
        int idCounter = 1;
        for (int i = 0; i < storageHeight; i++)
        {
            for (int j = 0; j < storageWidth; j++)
            {
                storage[j, i].spaceID = idCounter++;
                storage[j, i].ingredient = '\0';
                storage[j, i].remainPeriod = 0;
                storage[j, i].groceriesLeft = 0;
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
