using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STATE = EnumCollection.G_STATE;

public class PayAtCounter : MonoBehaviour
{

    //child(0) : Counter Position For Cast
    //child(1) : Pay Position For Guest

    public static float totalIncome = 0.0f;

    private GameObject targetPlayer;
    private GameObject accessGround;
    private Queue<GameObject> guestQueue;
    private bool doWork;
    private float timeToPay;
    private float timer;

    void Start()
    {
        guestQueue = new Queue<GameObject>();
        targetPlayer = null;
        doWork = false;
        timer = 0.0f;
        timeToPay = 0.0f;
    }


    void Update()
    {

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저 플래그 발생
            targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            GetComponent<ObjectEventManager>().doEvent = false;
            accessGround = GetCounterGroundForCast();
            if (targetPlayer != null)
            {   //castPosition에 있는 땅으로 이동시킴
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(accessGround);
                timeToPay = targetPlayer.GetComponent<PlayerConfig>().timeToPay;
                doWork = true;
            }
        }
        if (doWork && targetPlayer != null)
        {
            ProcessingEvent();
        }

    }

    private void ProcessingEvent()
    {
        if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != accessGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWork = false;
            timer = 0.0f;
            targetPlayer = null;
            timeToPay = 0.0f;
            return;
        }

        //플레이어가 도착하면
        if (targetPlayer.transform.position == accessGround.transform.position)
        {
            //대기 큐가 존재하면
            if (guestQueue.Count > 0)
            {
                timer += Time.deltaTime;
                if (timer >= timeToPay)
                {
                    GuestAction guest = guestQueue.Dequeue().GetComponent<GuestAction>();
                    for (int i = 0; i < guest.myMenus.Count; i++)
                    {
                        //모든 메뉴에 대한 가격을 더함
                        totalIncome += MenuData.GetPriceOfMenu(guest.myMenus[i]);
                    }
                    //손님을 퇴장시킴
                    //isPayer가 false이고 현재 상태가 EAT이면 자동으로 퇴장하게 되어있음
                    guest.isPayer = false;
                    guest.currentState = STATE.EAT;
                    timer = 0;
                    doWork = false;
                    targetPlayer = null;

                    Queue<GameObject> tmpQueue = new Queue<GameObject>();
                    //guestQueue가 비어질 때까지 tmpQueue에 복사
                    while (guestQueue.Count > 0)
                    {
                        tmpQueue.Enqueue(guestQueue.Dequeue());
                    }
                    //tmpQueue가 비어질 때까지 반복
                    while (tmpQueue.Count > 0)
                    {
                        //다시 guestQueue를 채워넣으면서 이동시킴
                        GameObject payer = tmpQueue.Dequeue();
                        payer.GetComponent<GuestAction>().currentState = STATE.MOVE;
                        payer.GetComponent<GuestAction>().MoveThisGround(
                            EnqueueGuestForPay(payer)
                            );
                    }
                }
            }
        }
    }

    private GameObject GetPayGroundForGuest()
    {
        Vector2 guestPosition = transform.GetChild(1).position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(guestPosition, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    private GameObject GetCounterGroundForCast()
    {
        Vector2 castPosition = transform.GetChild(0).position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(castPosition, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    private GameObject GetGround(Vector2 _pos)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(_pos, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    public GameObject EnqueueGuestForPay(GameObject _guest)
    {
        if (guestQueue.Contains(_guest)) { return null; }
        guestQueue.Enqueue(_guest);
        //대기열 순서대로 줄을 서게 함
        Vector2 pos = (Vector2)transform.GetChild(1).position + Vector2.down * (guestQueue.Count - 1);
        Debug.Log(guestQueue.Count);
        Debug.Log(pos);
        return GetGround(pos);
    }

    public GameObject PeekGuestFromQueue() { return guestQueue.Peek(); }

}
