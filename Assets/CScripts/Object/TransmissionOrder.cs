using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionOrder : MonoBehaviour
{
    private List<GameObject> targetPlayer;
    private GameObject deskGround;
    private List<bool> doWork;

    // Use this for initialization
    void Start()
    {
        targetPlayer = new List<GameObject>();
        doWork = new List<bool>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저 플래그 발생
            if (!targetPlayer.Contains(GetComponent<ObjectEventManager>().currentPlayer))
            {
                targetPlayer.Add(GetComponent<ObjectEventManager>().currentPlayer);
                doWork.Add(false);
            }

            deskGround = GetDeskGround();
            GetComponent<ObjectEventManager>().doEvent = false;

            for (i = 0; i < targetPlayer.Count; i++)
            {
                if (targetPlayer[i] != null)
                {   //대상 플레이어 이동
                    doWork[i] = true;
                    targetPlayer[i].GetComponent<MyCharacterMove>().MoveThisGround(deskGround);
                }
            }
        }

        for (i = 0; i < targetPlayer.Count; i++)
        {
            if (doWork[i] && targetPlayer[i] != null)
            {   //이벤트에 대한 처리
                ProcessingEvent(i);
            }
        }
    }

    private GameObject GetDeskGround()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.GetChild(0).position, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    private void ProcessingEvent(int _index)    //이벤트 처리
    {
        if (targetPlayer[_index].GetComponent<MyCharacterMove>().GetTargetGround() != deskGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWork[_index] = false;
            targetPlayer.RemoveAt(_index);
            doWork.RemoveAt(_index);
            return;
        }

        if (targetPlayer[_index].transform.position == deskGround.transform.position)
        {   //플레이어가 도착하면
            targetPlayer[_index].GetComponent<PlayerConfig>().SendOrderToUI();
            targetPlayer.RemoveAt(_index);
            doWork.RemoveAt(_index);
        }

    }


}
