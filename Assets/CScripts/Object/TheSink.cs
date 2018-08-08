using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TITLE = EnumCollection.TITLE;

public class TheSink : MonoBehaviour
{
    //Parent : The Sink
    //- child(0) : sprite(this Object)
    //- child(1) : Marker
    //- child(2) : AccessPoint
    public GameObject kitchenUI;

    private GameObject targetPlayer;
    private GameObject marker;

    void Start()
    {
        targetPlayer = null;
        marker = transform.parent.GetChild(1).gameObject;
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
            ObjectEventManager.CancelAllSelectWithOutObject(transform.parent.gameObject);  //다른 오브젝트 선택 해제
            Camera.main.GetComponent<EventListener>().ResetSelectPlayer();  //플레이어 선택 해제
            GetComponent<ObjectEventManager>().doEvent = false;
            //Marker 활성화/비활성화
            marker.GetComponent<SpriteRenderer>().enabled = !marker.GetComponent<SpriteRenderer>().enabled;

            if (marker.GetComponent<SpriteRenderer>().enabled)
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.DISHING);   //Marker 활성화 시 주방의 UI를 Dishing으로 전환
            }
            else
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.NONE);   //Marker 비활성화 시 주방의 UI를 초기화
            }

        }
    }
}
