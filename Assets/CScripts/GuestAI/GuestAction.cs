using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestAction : MyCharacterMove{
    /*STATE LIST : Move , Hold , Sit , Order , Wait , Eat , Pay
     */
    public string currentState;
    public float timeToOrderSec;

    private GameObject myTable;
    private GameObject myGroup;
    private float timer;

	// Use this for initialization
	void Start ()
    {
        timer = 0.0f;
    }

    void Update()
    {
        switch (currentState)
        {
            case "Move":    //이동 처리
                MoveStateAction();
                break;
            case "Hold":    //목표 지점까지 이동 완료 -> Move 다음 행동 분기점
                HoldStateAction();
                break;
            case "Sit":     //착석
                SitStateAction();
                break;
            case "Order":
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
                currentState = "Hold";
            }
        }

        if (path == null)
        {
            counter = -1;
            isMoving = false;
        }
    }


    void HoldStateAction()
    {
        GameObject[] groundList = myTable.GetComponent<AccessPositionOfObject>().GetAllGround();

        for (int i = 0; i < groundList.Length; i++)
        {
            if (RayCastGround(transform.position) == groundList[i])
            {
                currentState = "Sit";
                if (myGroup.GetComponent<GuestAction>().currentState == "Sit")
                {   //일행도 앉아있다면 테이블의 상태를 USING(1)로 전환
                    myTable.GetComponent<TableStateManager>().stateOfTable = (int)TableStateManager.STATE.USING;
                }
                groundList[i].GetComponent<GroundUnit>().SetAbsolutePassable(false);    //손님이 자리잡은 땅을 절대 지나갈 수 없게 함
            }
        }
    }


    void SitStateAction()
    {
        timer += Time.deltaTime;
        if (timer >= timeToOrderSec)   //일정 시간이 지나면 주문
        {
            timer = 0;
            currentState = "Order";
            //같은 부모를 둔 자식들 중, Marker의 스프라이트 활성화
            myTable.transform.parent.GetChild(1).gameObject.SetActive(true);
            myTable.GetComponent<TableStateManager>().SetOrderEventEnabled(true);
        }
    }


    void OrderStateAction()
    {

    }

    public void SetCurrentState(string _state) { currentState = _state; }

    public void SetFieldObject(GameObject _field) { currentField = _field; }

    public void SetMyTable(GameObject _table) { myTable = _table; }

    public void SetMyGroup(GameObject _group) { myGroup = _group; }

}
