using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using T_STATE = EnumCollection.T_STATE;
using G_STATE = EnumCollection.G_STATE;

public class GuestAction : MyCharacterMove{
    /*STATE LIST : Move , Hold , Sit , Order , Wait , Eat , Pay
     */

    public GameObject counterToPay;
    public G_STATE currentState;
    public float timeToOrderSec;
    public float timeToEatSec;
    public bool isPayer;

    private G_STATE prevState;
    private GameObject myTable;
    private GameObject myGroup;
    private GameObject myGround;
    private GuestConfig config;
    private float timer;
    private bool waitForOrder;

	// Use this for initialization
	void Start ()
    {
        timer = 0.0f;
        waitForOrder = false;
        prevState = G_STATE.MOVE;
        counterToPay = GameObject.Find("CounterSprite");
    }

    void Update()
    {
        switch (currentState)
        {
            case G_STATE.MOVE:    //이동 처리
                MoveStateAction();
                break;
            case G_STATE.HOLD:    //목표 지점까지 이동 완료 (Move 다음 행동 분기점)
                HoldStateAction();
                break;
            case G_STATE.SIT:     //착석
                SitStateAction();
                break;
            case G_STATE.ORDER: //주문 요청
                OrderStateAction();
                break;
            case G_STATE.EAT:
                EatStateAction();
                break;
            case G_STATE.PAY:
                PayStateAction();
                break;
        }

        if (prevState != currentState)
        {
            if (myTable != null)
            {
                myTable.transform.parent
                    .GetComponentInChildren<TableActMarker>().ShowActionMarker(currentState);
            }
            prevState = currentState;
        }
    }


    void MoveStateAction()
    {
        if (isMoving && path != null)
        {   //경로가 존재하며 이동 명령을 받은 경우

            if (counter >= 0)
            {   //목적지 도달까지

                thisPosition = transform.position;

                if (path[counter].GetPosition() != thisPosition)
                {   //이동
                    iTween.MoveTo(gameObject, iTween.Hash("x", path[counter].GetPosition().x, "y", path[counter].GetPosition().y, "speed", moveSpeed, "easetype", iTween.EaseType.linear));
                }
                else
                {
                    counter--;
                }
            }
            //목적지 도달 시
            else
            {
                isMoving = false;
                path.Clear();
                currentState = G_STATE.HOLD;
            }
        }

        if (path == null)
        {
            counter = -1;
            isMoving = false;
        }
    }   //Move 상태 AI


    void HoldStateAction()
    {
        //그룹이 없는 상태 -> 식사가 끝나고 이동을 마친 상태
        if (myGroup == null)
        {
            //지불자이면 카운터의 대기열에 서게 됨.
            if (isPayer)
            {
                currentState = G_STATE.PAY;
            }
            //지불자가 아니면 게임 오브젝트 제거(퇴장)
            else
            {
                Destroy(gameObject);
            }
            return;
        }

        GameObject[] groundList = myTable.GetComponent<AccessPositionOfObject>().GetAllGround();

        for (int i = 0; i < groundList.Length; i++)
        {
            if (RayCastGround(transform.position) == groundList[i])
            {
                myGround = groundList[i];
                currentState = G_STATE.SIT;
                if (myGroup.GetComponent<GuestAction>().currentState == G_STATE.SIT)
                {   //일행도 앉아있다면 테이블의 상태를 USING(1)로 전환
                    myTable.GetComponent<TableStateManager>().stateOfTable = T_STATE.USING;
                }
                groundList[i].GetComponent<GroundUnit>().SetAbsolutePassable(false);    //손님이 자리잡은 땅을 절대 지나갈 수 없게 함
            }
        }
    }   //Hold 상태 AI


    void SitStateAction()
    {
        timer += Time.deltaTime;
        if (timer >= timeToOrderSec)   //일정 시간이 지나면 주문
        {
            timer = 0;
            currentState = G_STATE.ORDER;
        }
    }   //Sit 상태 AI


    void OrderStateAction()
    {
        if (!waitForOrder)
        {
            //주문의 정보를 Table에 넘김
            myTable.GetComponent<TableStateManager>().SetOrderEventEnabled(true);
            waitForOrder = true;
        }
    }   //Order 상태 AI


    void EatStateAction()   //Eat 상태 AI
    {
        timer += Time.deltaTime;
        if (timer >= timeToEatSec)
        {
            TableStateManager table = myTable.GetComponent<TableStateManager>();
            table.DecideWhoPay();
            table.stateOfTable = T_STATE.DIRTY;
            table.guestGroup.Clear();
            currentState = G_STATE.MOVE;
            timer = 0;
            table.transform.parent.GetComponentInChildren<TableActMarker>().ShowActionMarker(currentState);
            myTable = null;
            myGroup = null;
            myGround.GetComponent<GroundUnit>().SetAbsolutePassable(true);

            GameObject moveToGround = null;
            if (isPayer)
            {
                moveToGround = counterToPay.GetComponent<PayAtCounter>().EnqueueGuestForPay(gameObject);
                MoveThisGround(moveToGround);
            }
            else
            {
                moveToGround = RayCastGround(GameObject.Find("GuestSpawn").transform.position);
                MoveThisGround(moveToGround);
            }
        }

    }


    void PayStateAction()   //Pay 상태 AI
    {
        if (counterToPay.GetComponent<PayAtCounter>().PeekGuestFromQueue()
            != gameObject)
        {
            currentState = G_STATE.MOVE;
            MoveThisGround(RayCastGround((Vector2)transform.position + Vector2.up));
        }
    }

    public void SetCurrentState(G_STATE _state) { currentState = _state; }

    public void SetMyGroup(GameObject _group) { myGroup = _group; }

    public void InitGuest(GameObject _table, GameObject _field)
    {
        myTable = _table;
        currentField = _field;
        config = new GuestConfig(myTable);
    }

    public GuestConfig GetGuestConfig() { return config; }

    public GameObject GetMyTable() { return myTable; }

}
