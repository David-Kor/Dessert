using UnityEngine;
using TITLE = EnumCollection.TITLE;

public class CookingStand : MonoBehaviour
{
    public GameObject kitchenUI;
    public GameObject storage;

    private bool doWorkEvent;
    private bool doCook;
    private bool endCook;
    private bool doDecreased;
    private GameObject targetPlayer;
    private GameObject accessGround;
    private GameObject marker;
    private MenuData.Menu menu;
    private float timer;

    void Start()
    {
        targetPlayer = null;
        menu = null;
        marker = transform.parent.GetChild(1).gameObject;
        timer = 0f;
        doDecreased = true;
    }

    void Update()
    {

        if (GetComponent<ObjectEventManager>().doCancel)    //선택 취소
        {
            GetComponent<ObjectEventManager>().doCancel = false;
            marker.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (GetComponent<ObjectEventManager>().doEvent) //클릭 이벤트 발생
        {
            ObjectEventManager.CancelAllSelectWithOutObject(transform.parent.gameObject);  //다른 오브젝트 선택 해제
            Camera.main.GetComponent<EventListener>().ResetSelectPlayer();  //플레이어 선택 해제
            GetComponent<ObjectEventManager>().doEvent = false;
            //Marker 활성화/비활성화
            marker.GetComponent<SpriteRenderer>().enabled = !marker.GetComponent<SpriteRenderer>().enabled;

            if (marker.GetComponent<SpriteRenderer>().enabled)
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.COOK);   //Marker 활성화 시 주방의 UI를 Cook로 전환
            }
            else
            {
                kitchenUI.GetComponent<KitchenUI>().ChangeViewUI(TITLE.NONE);   //Marker 비활성화 시 주방의 UI를 초기화
            }

        }

        if (targetPlayer != null)
        {
            if (doWorkEvent)    //작업 이벤트 발생
            {
                doWorkEvent = false;
                doCook = true;
                accessGround = GetAccessGround();
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(accessGround);
            }

            if (doCook && menu != null) { ProcessingWorkEvent(); }
            else if (endCook)
            {
                endCook = false;
                CompleteCookMenu();
            }
        }

    }

    GameObject GetAccessGround()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.parent.GetChild(2).position, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground"))
            {
                return hit[i].collider.gameObject;
            }
        }

        return null;
    }

    void ProcessingWorkEvent()
    {
        if (targetPlayer.GetComponent<MyCharacterMove>().GetTargetGround() != accessGround)     //목적지가 바뀌면
        {
            doCook = false;
            doDecreased = false;
            targetPlayer = null;
            menu = null;
            timer = 0;
            return;
        }

        if (targetPlayer.transform.position == accessGround.transform.position)
        {
            StartCookMenu();
        }

    }

    void CompleteCookMenu() //요리 완료
    {
        targetPlayer = null;
        timer = 0;
        bool preActive = kitchenUI.transform.GetChild((int)TITLE.COOK + 1).gameObject.activeSelf;   //원래 활성화 상태 저장
        kitchenUI.transform.GetChild((int)TITLE.COOK + 1).gameObject.SetActive(true);
        CookingUI cookingUI = kitchenUI.GetComponentInChildren<CookingUI>();
        GameObject orderPanel = cookingUI.FindMenuInOrderPanel(menu);
        orderPanel.GetComponent<CookOrderPanelUI>().CompleteThisOrder();
        kitchenUI.transform.GetChild((int)TITLE.COOK + 1).gameObject.SetActive(preActive);
        menu = null;
    }

    void StartCookMenu()   //요리 명령에 관한 처리
    {
        timer += Time.deltaTime;
        //요리 시작 시 재료의 감소
        if (doDecreased)
        {
            storage.GetComponent<Storage>().DecreaseIngredient(menu);
            doDecreased = false;
        }

        if (timer >= menu.time)
        {
            targetPlayer.GetComponentInChildren<PlayerCharacterAnimation>().isWorking = false;
            doCook = false;
            endCook = true;
        }
    }

    public bool OrderCookingToPlayer(GameObject _player, MenuData.Menu _menu)  //요리 시작 명령을 받음
    {
        if (_menu != null && _player != null && !doCook)
        {
            menu = _menu;
            for (int i = 0; i < menu.ingredients.Count; i++)
            {
                if (storage.GetComponent<Storage>().totalInfo[menu.ingredients[i].sign]   //현재 이 재료의 재고량
                    < menu.ingredients[i].require)  //이 메뉴를 만드는데 필요한 재료의 양
                {
                    menu = null;
                    return false;
                }
            }
            targetPlayer = _player;
            doWorkEvent = true;
            doDecreased = true;
            return true;
        }

        return false;
    }



}
