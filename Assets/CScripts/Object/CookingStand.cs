using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TITLE = EnumCollection.TITLE;

public class CookingStand : MonoBehaviour {
    public GameObject kitchenUI;
    private bool doWork;
    private GameObject targetPlayer;
    private GameObject accessGround;
    private GameObject marker;

    void Start()
    {
        targetPlayer = null;
        marker = transform.parent.GetChild(1).gameObject;
    }
	
	void Update () {
        if (GetComponent<ObjectEventManager>().doEvent) //클릭 이벤트 발생
        {
            GetComponent<ObjectEventManager>().doEvent = false;
            //Marker 활성화/비활성화
            marker.GetComponent<SpriteRenderer>().enabled = !marker.GetComponent<SpriteRenderer>().enabled;
            Camera.main.GetComponent<EventListener>().ResetSelectPlayer();

            if (marker.GetComponent<SpriteRenderer>().enabled)
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.COOK);   //Marker 활성화 시 주방의 UI를 Cook로 전환
            }
            else
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.NONE);   //Marker 비활성화 시 주방의 UI를 초기화
            }

            if (doWork && targetPlayer != null)
            {   //targetPlayer를 지정할 필요 있음 (18.07.27 - 11:12)
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(GetAccessGround());
            }
        }
	}

    GameObject GetAccessGround()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll((Vector2)transform.parent.GetChild(2).position, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground"))
            {
                return hit[i].collider.gameObject;
            }
        }

        return null;
    }

    public void StartCookingThisPlayer(GameObject _player)  //조리 시작 명령을 받음
    {
        targetPlayer = _player;
        doWork = true;
    }

}
