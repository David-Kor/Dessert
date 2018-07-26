using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionOrder : MonoBehaviour
{
    private GameObject targetPlayer;
    private GameObject deskGround;
    private bool doWork;

    // Use this for initialization
    void Start()
    {
        targetPlayer = null;
        doWork = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저 플래그 발생
            targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            deskGround = GetDeskGround();
            GetComponent<ObjectEventManager>().doEvent = false;
            if (targetPlayer != null)
            {   //대상 플레이어 이동
                doWork = true;
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(deskGround);
            }
        }

        if (doWork && targetPlayer != null)
        {   //이벤트에 대한 처리
            ProcessingEvent();
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

    private void ProcessingEvent()
    {
        if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != deskGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWork = false;
        }

        if (targetPlayer.transform.position == deskGround.transform.position)
        {   //플레이어가 도착하면
            targetPlayer.GetComponent<PlayerConfig>().SendOrderToUI();
            targetPlayer = null;
            doWork = false;
        }
    }
}
