﻿using UnityEngine.UI;
using UnityEngine;

public class CookOrderPanelUI : MonoBehaviour
{
    /* [] : current Class */
    /* 부모 Object ────────────────────> 자식 Object */
    /* Kitchen UI > CookUI > PageUI + PageButton > [ OrderPanelUI ] > DetailPanelUI > SelectPlayerPanelUI */

    public Order order;
    //Child(0) -> TableID TEXT
    //Child(1) -> Menu Name TEXT
    //Child(2) -> DetailPanel
    void Awake()
    {
        order = null;
        transform.GetChild(0).GetComponentInChildren<Text>().text = "0";
        transform.GetChild(1).GetComponentInChildren<Text>().text = "아직 접수된 주문이 없습니다.";
    }

    public void OrderPanelTextWrite(Order _order, int _menuIndex)    //OrderPanelUI 및 DetailPanelUI에서 출력할 내용 설정
    {
        order = _order;
        //OrderPanelUI에서 출력할 Order 객체 정보 작성
        transform.GetChild(0).GetComponentInChildren<Text>().text = order.table.GetComponent<TableStateManager>().tableID.ToString();
        transform.GetChild(1).GetComponentInChildren<Text>().text = MenuData.ConvertMenuIDToName(order.menuIDList[_menuIndex]);

        //DetailPanelUI에서 출력할 Menu 객체 정보 전달
        bool detailActive = transform.GetChild(2).gameObject.activeSelf;
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetComponentInChildren<DetailsPanelUI>().DetailsPanelWrite(order.menuIDList[_menuIndex]);
        transform.GetChild(2).gameObject.SetActive(detailActive);   //원래 비활성화 상태였으면 다시 비활성화시킴
    }

    public void ResetDetails()   //OrderPanel 자신이 가진 DetailPanel을 비활성화
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ResetAllDetails()   //현재 페이지의 전체 DetailPanel을 비활성화
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }
    }

    public void ViewDetailsOnClick()    //OrderPanel 클릭 시 DetailPanel 활성화
    {
        bool active = !transform.GetChild(2).gameObject.activeSelf;
        ResetAllDetails();
        transform.GetChild(2).gameObject.SetActive(active);
    }

}
