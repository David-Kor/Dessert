using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CookingUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > [ CookUI ] > PageUI + PageButton > OrderPanelUI > DetailPanelUI */

    public GameObject orderStore;
    public GameObject originPage;
    public GameObject originButton;
    public GameObject currentPage;
    private LinkedList<GameObject> pageList;    //페이지의 각 자식들은 간략한 정보를 보여줄 한 칸의 UI에 해당함
    private LinkedList<GameObject> buttonList;
    private LinkedList<Order> orders;
    private int orderCounter;

    void Awake()
    {
        orderCounter = 0;
        pageList = new LinkedList<GameObject>();
        buttonList = new LinkedList<GameObject>();
        pageList.AddFirst(Instantiate(originPage, transform));  //초기 1페이지 생성
        pageList.First.Value.transform.GetChild(0).gameObject.SetActive(true);  //1번째 패널 활성화
        buttonList.AddFirst(Instantiate(originButton, transform)).Value.GetComponent<PageButtonUI>().myPage = pageList.First.Value;  //1페이지 버튼 생성
        currentPage = pageList.First.Value;
        currentPage.SetActive(true);
        orders = orderStore.GetComponent<OrderInfoList>().GetOrderList();
    }

    void Update()
    {

    }

    public void AppendNodeOrderUI(Order _order)
    {
        LinkedListNode<GameObject> pageNode = pageList.First;
        int menuIndex = 0;

        //페이지 단위 반복
        while (menuIndex < _order.menuIDList.Count)
        {
            //자식 패널단위 반복
            for (int j = 0; j < pageNode.Value.transform.childCount; j++)   //pageNode의 자식 오브젝트 검사
            {

                if (orderCounter == 0)
                {   //처음 들어온 주문인 경우 비활성화 후 정보를 작성하고 다시 활성화시킴
                    pageNode.Value.transform.GetChild(0).gameObject.SetActive(false);
                }

                if (!pageNode.Value.transform.GetChild(j).gameObject.activeSelf)
                {   //비활성화 되어있으면 활성화 시키고 해당 정보 텍스트 작성
                    pageNode.Value.transform.GetChild(j).gameObject.SetActive(true);
                    WriteOrderInfo(pageNode.Value.transform.GetChild(j).gameObject, _order, menuIndex++);
                    orderCounter++;
                }

                if (menuIndex >= _order.menuIDList.Count)     //모든 메뉴가 다 입력된 경우
                {
                    //현재 보고 있는 페이지가 아니면 비활성화하고 마침
                    if (currentPage != pageNode.Value) { pageNode.Value.SetActive(false); }
                    return;
                }

            }

            //현재 보고 있는 페이지가 아니면 비활성화
            if (currentPage != pageNode.Value) { pageNode.Value.SetActive(false); }
            //다음 페이지가 없으면 페이지와 버튼을 새로 생성
            if (pageNode.Next == null)
            {
                pageList.AddLast(Instantiate(originPage, transform));
                buttonList.AddLast(Instantiate(originButton, transform)).Value.transform.localPosition += Vector3.right * 70.0f * (buttonList.Count - 1);
                buttonList.Last.Value.GetComponentInChildren<Text>().text = buttonList.Count.ToString();
                buttonList.Last.Value.GetComponent<PageButtonUI>().myPage = pageList.Last.Value;
            }

            pageNode = pageNode.Next;
        }

    }

    void WriteOrderInfo(GameObject _panelUI, Order _order, int _menuIndex)   //패널 UI에 주문 정보 작성
    {
        string tableID = _order.table.GetComponent<TableStateManager>().tableID.ToString() + " T";
        string name = MenuData.ConvertMenuIDToName(_order.menuIDList[_menuIndex]);

        _panelUI.GetComponent<CookOrderPanelUI>().OrderPanelTextWrite(tableID, name, _order.menuIDList[_menuIndex]);
    }

    public void SetCurrentPage(GameObject _page)    //현재 보여줄 페이지를 설정
    {
        if (_page == null) { return; }

        LinkedListNode<GameObject> node = pageList.First;
        currentPage = _page;

        int i = 1;
        while (node != null)
        {
            for (int j = 0; j < node.Value.transform.childCount; j++)
            {
                node.Value.SetActive(true);     //잠깐 활성화 시킨 후 컴포넌트를 받아옴
                node.Value.transform.GetChild(j).GetComponent<CookOrderPanelUI>().ResetDetails();
                node.Value.SetActive(false);
            }
            //_page를 제외한 나머지 페이지들을 비활성화
            if (node.Value == _page) { node.Value.SetActive(true); }
            else { node.Value.SetActive(false); }
            node = node.Next;
            i++;
        }
    }

    public LinkedList<Order> GetOrderList() { return orders; }

}
