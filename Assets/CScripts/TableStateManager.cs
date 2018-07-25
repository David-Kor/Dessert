using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_STATE = EnumCollection.G_STATE;
using T_STATE = EnumCollection.T_STATE;

public class TableStateManager : MonoBehaviour {
    public T_STATE stateOfTable;    //테이블 상태 -> 0: 준비(청소됨), 1: 사용중, 2: 사용 끝(청소 안됨), 3: 사용하려는 손님이 있음
    public int tableID;
    public float timeToScrub;
    public float timeToOrder;
    public List<GameObject> guestGroup;
    public static int MAX_ORDER_COUNT = 2;

    private T_STATE preState;
    private bool doWork;
    private bool isGuestOrder;
    private float timer;
    private GameObject targetPlayer;
    private GameObject accessGround;
    private List<int> orderList;

    // Use this for initialization
    void Start()
    {
        stateOfTable = 0;
        preState = T_STATE.CLEAR;
        doWork = false;
        targetPlayer = null;
        accessGround = null;
        guestGroup = new List<GameObject>();
        orderList = new List<int>();

        string strToID = transform.parent.gameObject.name;
        strToID = strToID.Split(' ')[1];
        strToID = strToID.Replace("(", "");     // ( 제거
        strToID = strToID.Replace(")", "");     // ) 제거
        tableID = System.Convert.ToInt32(strToID) + 1;
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

            if (targetPlayer != null && stateOfTable != T_STATE.CLEAR)
            {   //작업대상 플레이어를 움직임
                doWork = true;
                if (!isGuestOrder && stateOfTable == T_STATE.USING) doWork = false;
                if (targetPlayer.transform.position == accessGround.transform.position) { return; }
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(accessGround);
                timeToOrder = targetPlayer.GetComponent<PlayerConfig>().timeToTakeOrder;
                timeToScrub = targetPlayer.GetComponent<PlayerConfig>().timeToScrub;
            }

        }

        if (doWork && targetPlayer != null)
        {   //이벤트에 대한 처리
            ProcessingEvent();
        }

        if (isGuestOrder)
        {   //주문 요청 처리
            bool isReady = true;   //같은 테이블의 모든 손님의 상태가 ORDER인지 여부
            for (int i = 0; i < guestGroup.Count; i++)
            {
                if (guestGroup[i].GetComponent<GuestAction>().currentState != G_STATE.ORDER)
                { isReady = false; }
            }

            if (isReady)
            {
                if (!transform.parent.GetChild(1).GetComponent<SpriteRenderer>().enabled)
                {
                    transform.parent.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                }

                //이 테이블의 주문 목록이 비었을 경우 주문할 목록을 받아옴
                if (orderList.Count == 0) { RequestOrder(); }
            }
        }
    }

    void StateChanged() //상태 변화 처리
    {
        preState = stateOfTable;

        switch (stateOfTable)
        {
            case T_STATE.CLEAR:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case T_STATE.USING:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case T_STATE.DIRTY:
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
            case T_STATE.RESERVED:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
        }
    }

    void ProcessingEvent()  //플레이어(클릭)에 대한 이벤트 처리
    {
        if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != accessGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWork = false;
            timer = 0.0f;
        }

        if (targetPlayer.transform.position == accessGround.transform.position)
        {   //플레이어가 도착하면
            switch (stateOfTable)
            {
                case T_STATE.DIRTY:
                    targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isScrubbing = true;
                    timer += Time.deltaTime;
                    if (timer >= timeToScrub)
                    {
                        doWork = false;
                        stateOfTable = T_STATE.CLEAR;
                        targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isScrubbing = false;
                        timer = 0.0f;
                    }
                    break;

                case T_STATE.USING:
                    if (isGuestOrder && orderList.Count > 0)
                    {   //주문할 메뉴가 정해져 있다면
                        timer += Time.deltaTime;
                        if (timer >= timeToOrder)
                        {
                            transform.parent.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;

                            for (int i = 0; i < guestGroup.Count; i++)
                            {   //그룹에 속한 손님들의 상태를 WAIT로 전환
                                guestGroup[i].GetComponent<GuestAction>().SetCurrentState(G_STATE.WAIT);
                            }
                            targetPlayer.GetComponent<PlayerConfig>().TakeOrderToPlayer(gameObject, orderList);
                            doWork = false;
                            isGuestOrder = false;
                            targetPlayer = null;
                        }
                    }
                    break;
            }
        }
    }

    void RequestOrder() //주문 요청에 관한 처리
    {
        int i = 0;
        int j = 0;
        MenuData menuData = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuData>();
        GuestConfig config = null;

        for (i = 0; i < guestGroup.Count; i++)
        {
            config = guestGroup[i].GetComponent<GuestAction>().GetGuestConfig();    //그룹에 속한 손님의 정보를 받아옴
            List<int> myMenu = menuData.GetIDsContainsIngredients(config.favorite, MAX_ORDER_COUNT);

            for (j = 0; j < myMenu.Count; j++)
            {
                if (!menuData.GetMenu(myMenu[j]).enabled)
                {   //비활성화 메뉴는 제거
                    myMenu.RemoveAt(j);
                    j--;
                }
            }

            if (myMenu.Count == 0)  //주문할 수 있는 메뉴가 없으면 활성화 메뉴 중에서 임의로 선택
            {
                int rand = Random.Range(0, menuData.enableMenus.Count);
                myMenu.Add(menuData.enableMenus[rand].id);
            }

            //메뉴 병합(중복 허용)
            for (j = 0; j < myMenu.Count; j++)
            {
                orderList.Add(myMenu[j]);
            }
        }

    }

    public void SetOrderEventEnabled(bool _enabled) //이벤트 발생 여부 받기
    {
        isGuestOrder = _enabled;
    }

}
