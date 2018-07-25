using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToWarpSpace : MonoBehaviour
{
    private GameObject warpToPoint;
    private GameObject warpFromPoint;
    private List<GameObject> targetPlayer;
    private List<bool> doWarp;
    private GameObject accessGround;
    private Vector2 dist;

    void Start()
    {
        warpToPoint = transform.GetChild(0).gameObject;
        warpFromPoint = transform.GetChild(1).gameObject;
        doWarp = new List<bool>();
        targetPlayer = new List<GameObject>();
    }


    void Update()
    {
        int i;

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저 플래그 발생
            if (!targetPlayer.Contains(GetComponent<ObjectEventManager>().currentPlayer))
            {
                targetPlayer.Add(GetComponent<ObjectEventManager>().currentPlayer);
                doWarp.Add(false);
            }
            accessGround = GetWarpGround();
            GetComponent<ObjectEventManager>().doEvent = false;
            for (i = 0; i < targetPlayer.Count; i++)
            {
                if (targetPlayer[i] != null)
                {   //대상 플레이어 이동
                    doWarp[i] = true;
                    targetPlayer[i].GetComponent<MyCharacterMove>().MoveThisGround(accessGround);
                }
            }
        }
        for (i = 0; i < targetPlayer.Count; i++)
        {
            if (doWarp[i] && targetPlayer[i] != null)
            {   //이벤트에 대한 처리
                ProcessingEvent(i);
            }
        }
    }

    void ProcessingEvent(int _index)
    {
        if (targetPlayer[_index].GetComponent<MyCharacterMove>().GetTargetGround() != accessGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWarp[_index] = false;
        }

        if (targetPlayer[_index].transform.position == accessGround.transform.position
            && !targetPlayer[_index].GetComponent<MyCharacterMove>().GetIsMoving())
        {   //플레이어가 도착하면
            targetPlayer[_index].transform.position = warpToPoint.transform.position;
            targetPlayer[_index].GetComponent<MyCharacterMove>().CheckField();
            if (Camera.main.GetComponent<EventListener>().GetSelectedPlayer() == targetPlayer[_index])
            {
                Camera.main.GetComponent<EventListener>().ResetSelectPlayer();
            }
            targetPlayer.RemoveAt(_index);
            doWarp.RemoveAt(_index);
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
