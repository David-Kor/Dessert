using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToWarpSpace : MonoBehaviour
{
    private GameObject warpToPoint;
    private GameObject warpFromPoint;
    private GameObject targetPlayer;
    private bool doWarp;
    private Vector2 dist;

    void Start()
    {
        warpToPoint = transform.GetChild(0).gameObject;
        warpFromPoint = transform.GetChild(1).gameObject;
        doWarp = false;
    }


    void Update()
    {
        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저의 플래그 변수가 true인 경우
            GetComponent<ObjectEventManager>().doEvent = false;
            targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            if (targetPlayer != null)
            {
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(GetWarpGround());
                doWarp = true;
            }
        }

        if (doWarp && targetPlayer != null)
        {
            if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != GetWarpGround())
            {   //대상 플레이어의 목적지가 바뀌면
                doWarp = false;
            }

            if (targetPlayer.transform.position == GetWarpGround().transform.position)
            {   //대상 플레이어가 워프 포인트에 도착하면
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(null);
                targetPlayer.transform.position = warpToPoint.transform.position;
                targetPlayer.GetComponent<MyCharacterMove>().CheckField();
                if (Camera.main.GetComponent<EventListener>().GetSelectedPlayer() == targetPlayer)
                {
                    Camera.main.GetComponent<EventListener>().ResetSelectPlayer();
                }
                doWarp = false;
                targetPlayer = null;
            }

        }

    }


    GameObject GetWarpGround()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(warpFromPoint.transform.position, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground"))
            {
                return hit[i].collider.gameObject;
            }
        }

        return null;

    }


}
