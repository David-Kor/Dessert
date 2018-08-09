using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_STATE = EnumCollection.G_STATE;

public class TableActMarker : MonoBehaviour {
    public Sprite[] waitingImg;
    public Sprite orderImg;
    public Sprite eatingImg;

    private SpriteRenderer myImg;

    void Start() { myImg = GetComponent<SpriteRenderer>(); }

    public void ShowActionMarker(G_STATE _state)
    {
        switch (_state)
        {
            //식사 상태 이미지 적용
            case G_STATE.EAT:
                {
                    myImg.sprite = eatingImg;
                    break;
                }

            //대기 상태 이미지중에서 임의로 선택하여 적용
            case G_STATE.SIT:
                {
                    myImg.sprite = waitingImg[Random.Range(0, waitingImg.Length)];

                    //이 테이블의 그룹에 속한 모든 손님들이 똑같이 SIT상태가 되어야만 적용
                    List<GameObject> guestGroup = transform.parent.GetComponentInChildren<TableStateManager>().guestGroup;
                    for (int i = 0; i < guestGroup.Count; i++)
                    {
                        if (guestGroup[i].GetComponent<GuestAction>().currentState == G_STATE.SIT) { continue; }
                        myImg.sprite = null;
                    }
                    break;
                }
            case G_STATE.WAIT:
                {
                    myImg.sprite = waitingImg[Random.Range(0, waitingImg.Length)];
                    break;
                }
                
            //주문 상태 이미지 적용
            case G_STATE.ORDER:
                myImg.sprite = orderImg;
                break;

            default:
                myImg.sprite = null;
                myImg.enabled = false;
                break;
        }

        if (myImg.sprite != null) { myImg.enabled = true; }
    }

}
