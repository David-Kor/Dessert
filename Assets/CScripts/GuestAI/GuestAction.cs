using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using T_STATE = EnumCollection.T_STATE;
using G_STATE = EnumCollection.G_STATE;

public class GuestAction : MyCharacterMove{
    /*STATE LIST : Move , Hold , Sit , Order , Wait , Eat , Pay
     */
    public G_STATE currentState;
    public float timeToOrderSec;

    private GameObject myTable;
    private GameObject myGroup;
    private GuestConfig config;
    private float timer;
    private bool waitForOrder;

	// Use this for initialization
	void Start ()
    {
        timer = 0.0f;
        waitForOrder = false;
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
            case G_STATE.ORDER:
                OrderStateAction();
                break;
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
        GameObject[] groundList = myTable.GetComponent<AccessPositionOfObject>().GetAllGround();

        for (int i = 0; i < groundList.Length; i++)
        {
            if (RayCastGround(transform.position) == groundList[i])
            {
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

}
