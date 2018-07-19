using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableStateManager : MonoBehaviour {
    public int stateOfTable;    //테이블 상태 -> 0: 준비(청소됨), 1: 사용중, 2: 사용 끝(청소 안됨), 3: 사용하려는 손님이 있음
    public float timeToScrub;

    private int preState;
    private bool doWork;
    public bool isGuestOrder;
    private float timer;
    private GameObject targetPlayer;
    private GameObject accessGround;

    public enum STATE { CLEAR = 0, USING, DIRTY, RESERVED };

    // Use this for initialization
    void Start()
    {
        stateOfTable = 0;
        preState = 0;
        doWork = false;
        targetPlayer = null;
        accessGround = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (preState != stateOfTable)
        {   //상태가 변하는 경우
            StateChanged();
        }

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저의 플래그가 감지되면

            GetComponent<ObjectEventManager>().doEvent = false;
            targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            accessGround = GetComponent<AccessPositionOfObject>().GetUpGround();

            if (targetPlayer != null && stateOfTable != (int)STATE.CLEAR)
            {   //작업대상 플레이어를 움직임
                doWork = true;
                if (!isGuestOrder) doWork = false;
                if (targetPlayer.transform.position == accessGround.transform.position) { return; }
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(accessGround);
            }

        }

        if (doWork && targetPlayer != null)
        {   //이벤트에 대한 처리
            ProcessingEvent();
        }
    }

    void StateChanged()
    {
        preState = stateOfTable;

        switch ((STATE)stateOfTable)
        {
            case STATE.CLEAR:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case STATE.USING:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case STATE.DIRTY:
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
            case STATE.RESERVED:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
        }
    }

    void ProcessingEvent()
    {
        if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != accessGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWork = false;
            timer = 0.0f;
        }

        if (targetPlayer.transform.position == accessGround.transform.position)
        {   //플레이어가 도착하면
            switch ((STATE)stateOfTable)
            {
                case STATE.DIRTY:
                    targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isScrubbing = true;
                    timer += Time.deltaTime;
                    if (timer >= timeToScrub)
                    {
                        doWork = false;
                        stateOfTable = (int)STATE.CLEAR;
                        targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isScrubbing = false;
                        timer = 0.0f;
                    }
                    break;

                case STATE.USING:
                    if (isGuestOrder)
                    {
                        transform.parent.GetChild(1).gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    public void SetOrderEventEnabled(bool _enabled) { isGuestOrder = _enabled; }

}
