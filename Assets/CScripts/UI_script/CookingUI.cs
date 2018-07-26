using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingUI : MonoBehaviour {
    public GameObject orderStore;
    public GameObject originPage;
    public GameObject currentPage;
    private LinkedList<GameObject> pageList;    //페이지의 각 자식들은 간략한 정보를 보여줄 한 칸의 UI에 해당함
	void Start () {
        pageList = new LinkedList<GameObject>();
        pageList.AddFirst(Instantiate(originPage, transform));  //초기 1페이지 생성
        currentPage = pageList.First.Value;
	}

	void Update () {

	}

    public void AppendNodeOrderUI(Order _order)
    {
        LinkedListNode<GameObject> pageNode = pageList.First;
        int menuIndex = 0;

        for (int i = 0; i < _order.menuIDList.Count; i++)
        {
            Debug.Log(_order.table.GetComponent<TableStateManager>().tableID.ToString() + "  :  " +
               MenuData.ConvertMenuIDToName(_order.menuIDList[i]));
        }
        while (menuIndex < _order.menuIDList.Count)
        {
            for (int j = 0; j < pageNode.Value.transform.childCount; j++)   //pageNode의 자식 오브젝트 검사
            {
                if (menuIndex >= _order.menuIDList.Count)     //모든 메뉴가 다 입력된 경우
                {
                    //현재 보고 있는 페이지가 아니면 비활성화하고 마침
                    if (currentPage != pageNode.Value) { pageNode.Value.SetActive(false); }
                    return;
                }

                if (!pageNode.Value.transform.GetChild(j).gameObject.activeSelf)
                {   //비활성화 되어있으면 활성화 시키고 해당 정보 텍스트 작성
                    pageNode.Value.transform.GetChild(j).gameObject.SetActive(true);
                    WriteOuterViewOrderInfo(pageNode.Value.transform.GetChild(j).gameObject, _order, menuIndex++);
                }
            }
            //현재 보고 있는 페이지가 아니면 비활성화
            if (currentPage != pageNode.Value) { pageNode.Value.SetActive(false); }
            //다음 페이지가 없으면 새로 생성
            if (pageNode.Next == null) { pageList.AddLast(Instantiate(originPage, transform)); }

            pageNode = pageNode.Next;
        }
        
    }

    void WriteOuterViewOrderInfo(GameObject _ui, Order _order, int _menuIndex)   //주문 목록 정보 UI 텍스트 설정
    {
        string tableID = _order.table.GetComponent<TableStateManager>().tableID.ToString() + "T";
        string name = MenuData.ConvertMenuIDToName(_order.menuIDList[_menuIndex]);
        _ui.transform.GetChild(0).GetComponentInChildren<Text>().text = tableID;
        _ui.transform.GetChild(1).GetComponentInChildren<Text>().text = name;
    }
}
