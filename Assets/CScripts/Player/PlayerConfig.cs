using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour {

    public float timeToTakeOrder = 3.0f;
    public float timeToScrub = 5.0f;
    public Order servingOrder;

    private List<Order> myOrderList;    //<테이블, 주문 목록>쌍의 리스트

    void Start()
    {
        myOrderList = new List<Order>();
        servingOrder = null;
    }
    

    public void TakeOrderToPlayer(GameObject _table, List<int> _menuList)  //myOrderList에 주문받은 내용을 축적
    {
        myOrderList.Add(new Order
        {
            table = _table,
            menuIDList = _menuList,
        });
    }

    public void SendOrderToUI()
    {
        OrderInfoList orderInfo = GameObject.FindGameObjectWithTag("OrderList").GetComponent<OrderInfoList>();
        for (int i = 0; i < myOrderList.Count; i++)
        {
            orderInfo.AppendOrder(myOrderList[i]);
        }
        myOrderList.Clear();
    }

    public bool ReceieveOrderForServing(Order _order)
    {
        if (_order == null) { return false; }

        //플레이어가 이미 서빙중이 아닌 경우
        if (!GetComponentInChildren<PlayerCharacterAnimation>().isServing && servingOrder == null)
        {
            GetComponentInChildren<PlayerCharacterAnimation>().isServing = true;
            servingOrder = _order;
            GetComponentInChildren<TargetTablePointer>().targetTablePosition = servingOrder.table.transform.position;
            return true;
        }

        return false;
    }

    public List<Order> GetOrderList() { return myOrderList; }

}
