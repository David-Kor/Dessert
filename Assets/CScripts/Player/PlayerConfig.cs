using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour {

    public float timeToTakeOrder = 3.0f;
    public float timeToScrub = 5.0f;

    private List<Order> myOrderList;    //<테이블, 주문 목록>쌍의 리스트
    private Order servingOrder;

    void Start()
    {
        myOrderList = new List<Order>();
    }

    public void TakeOrderToPlayer(GameObject _table, List<int> _orderList)  //myOrderList에 주문받은 내용을 축적
    {
        myOrderList.Add(new Order
        {
            table = _table,
            menuIDList = _orderList,
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
}
