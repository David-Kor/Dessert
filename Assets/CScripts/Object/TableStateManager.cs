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

    const int MAX_MENU_COUNT = 2;   //한 손님당 받아올 메뉴의 최대 개수
    const int TABLE_ORDER_COUNT = 2;    //한 테이블당 주문할 수 있는 메뉴의 개수

    private T_STATE preState;
    private bool doWork;
    private bool isGuestOrder;
    private float timer;
    private GameObject targetPlayer;
    private GameObject accessGround;
    private List<int> orderMenus;
    private Color dirtyColor;



    void Start()
    {
        stateOfTable = 0;
        preState = T_STATE.CLEAR;
        doWork = false;
        targetPlayer = null;
        accessGround = null;
        guestGroup = new List<GameObject>();
        orderMenus = new List<int>();

        string strToID = transform.parent.gameObject.name;
        strToID = strToID.Split(' ')[1];
        strToID = strToID.Replace("(", "");     // ( 제거
        strToID = strToID.Replace(")", "");     // ) 제거
        tableID = System.Convert.ToInt32(strToID) + 1;
        dirtyColor = new Color(0.4f, 0.4f, 0.4f);
    }


    void Update()
    {
        if (preState != stateOfTable)
        {   //상태가 변하는 경우
            StateChanged();
        }

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저의 플래그가 감지되면

            GetComponent<ObjectEventManager>().doEvent = false;
            if (targetPlayer == null)
            {
                targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            }
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
                //이 테이블의 주문 목록이 비었을 경우 주문할 목록을 받아옴
                if (orderMenus.Count == 0) { RequestOrder(); }
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
                break;
            case T_STATE.WAITING:
                break;
            case T_STATE.DIRTY:
                GetComponent<SpriteRenderer>().color = dirtyColor;
                break;
            case T_STATE.RESERVED:
                break;
        }
    }

    void ProcessingEvent()  //플레이어(클릭)에 대한 이벤트 처리
    {
        if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != accessGround)
        {   //대상 플레이어의 목적지가 바뀌면
            doWork = false;
            timer = 0.0f;
            targetPlayer = null;
            return;
        }

        if (targetPlayer.transform.position == accessGround.transform.position)
        {   //플레이어가 도착하면
            switch (stateOfTable)
            {
                //더러운 상태일 때 처리
                case T_STATE.DIRTY:
                    {
                        //플레이어가 서빙중이라면 예외처리
                        if (targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isServing)
                        {
                            doWork = false;
                            timer = 0.0f;
                            targetPlayer = null;
                            return;
                        }

                        targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isScrubbing = true;
                        timer += Time.deltaTime;
                        if (timer >= timeToScrub)
                        {
                            doWork = false;
                            stateOfTable = T_STATE.CLEAR;
                            targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isScrubbing = false;
                            targetPlayer = null;
                            timer = 0.0f;
                        }
                        break;
                    }

                //사용중 상태일 때 처리
                case T_STATE.USING:
                    {
                        //플레이어가 서빙중이라면 예외처리
                        if (targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isServing)
                        {
                            doWork = false;
                            timer = 0.0f;
                            targetPlayer = null;
                            return;
                        }

                        if (isGuestOrder && orderMenus.Count > 0)
                        {   //주문할 메뉴가 정해져 있다면
                            timer += Time.deltaTime;
                            targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isTakeOrder = true;
                            if (timer >= timeToOrder)
                            {
                                targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isTakeOrder = false;

                                for (int i = 0; i < guestGroup.Count; i++)
                                {   //그룹에 속한 손님들의 상태를 WAIT로 전환
                                    guestGroup[i].GetComponent<GuestAction>().SetCurrentState(G_STATE.WAIT);
                                }
                                targetPlayer.GetComponent<PlayerConfig>().TakeOrderToPlayer(gameObject, orderMenus);

                                //테이블 상태를 WAITING으로 변경하고 변수들 초기화
                                stateOfTable = T_STATE.WAITING;
                                doWork = false;
                                isGuestOrder = false;
                                targetPlayer = null;
                            }
                        }
                        break;
                    }

                //대기중 상태일 때 처리
                case T_STATE.WAITING:
                    {
                        PlayerCharacterAnimation playerState = targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>();
                        PlayerConfig playerConfig = targetPlayer.GetComponent<PlayerConfig>();
                        //플레이어가 서빙 상태이고 주문을 시킨 테이블이 이 테이블 일 때
                        if (playerState.isServing
                            && playerConfig.servingOrder.table == gameObject)
                        {
                            playerState.isServing = false;
                            playerConfig.servingOrder = null;

                            //그룹에 속한 손님들의 상태를 EAT로 전환
                            for (int i = 0; i < guestGroup.Count; i++)
                            {
                                guestGroup[i].GetComponent<GuestAction>().SetCurrentState(G_STATE.EAT);
                            }
                        }

                        break;
                    }

            }

        }   //플레이어의 처리 완료


    }

    void RequestOrder() //주문 요청에 관한 처리
    {
        int i = 0;
        int j = 0;
        MenuData menuData = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuData>();
        GuestConfig config = null;
        List<int> myMenu = null;

        for (i = 0; i < guestGroup.Count; i++)
        {
            config = guestGroup[i].GetComponent<GuestAction>().GetGuestConfig();    //그룹에 속한 손님의 정보를 받아옴
            myMenu = menuData.GetIDsContainsIngredients(config.favorite, MAX_MENU_COUNT);

            for (j = 0; j < myMenu.Count; j++)
            {
                if (!MenuData.ConvertMenuIDToMenu(myMenu[j]).enabled)
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
        }

        //메뉴 병합(중복 허용)
        for (i = 0; i < myMenu.Count; i++)
        {
            orderMenus.Add(myMenu[i]);
        }

        while (orderMenus.Count > TABLE_ORDER_COUNT) //테이블 당 최대 주문 개수 맞추기
        {
            orderMenus.RemoveAt(Random.Range(0, orderMenus.Count));
        }

    }

    public void SetOrderEventEnabled(bool _enabled) //이벤트 발생 여부 받기
    {
        isGuestOrder = _enabled;
    }

    public static GameObject ConvertTableIDToGameObject(int _tableID)   //테이블 ID를 게임 오브젝트로 변환
    {
        //모든 테이블들의 ID를 검색
        TableStateManager[] tables = GameObject.Find("TableGroup").GetComponentsInChildren<TableStateManager>();
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i].tableID == _tableID)
            {   //찾은 테이블의 오브젝트를 반환(스프라이트용 오브젝트->자식0번)
                return tables[i].gameObject;
            }
        }

        return null;
    }

    public void DecideWhoPay()
    {
        int payer = Random.Range(0, guestGroup.Count);

        //손님들 중 임의로 한 명을 선택하여 지불하게 함
        for (int i = 0; i < guestGroup.Count; i++)
        {
            if (i == payer)
            {
                guestGroup[i].GetComponent<GuestAction>().isPayer = true;
            }
            else
            {
                guestGroup[i].GetComponent<GuestAction>().isPayer = false;
            }
        }
    }

    public List<int> GetOrderMenus() { return orderMenus; }

}
