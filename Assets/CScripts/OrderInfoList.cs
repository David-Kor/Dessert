using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInfoList : MonoBehaviour
{
    public GameObject cookUI;
    private LinkedList<Order> orders;
    private int preCount;
    private int idCounter;

    void Start()
    {
        orders = new LinkedList<Order>();
        preCount = 0;
        idCounter = 1;
    }
    void Update()
    {
        if (preCount != orders.Count)
        {
            preCount = orders.Count;
        }
    }

    public void AppendOrder(List<int> _idList, GameObject _table)   //직접 주문 정보를 받아 새 Order추가
    {
        orders.AddLast(new Order
        {
            orderID = idCounter++,
            table = _table,
            menuIDList = _idList,
            isComplete = false
        });
        SendNewOrderListToUI(orders.Last.Value);
    }

    public void AppendOrder(Order _order)   //Order클래스로 받아 새 Order추가
    {
        orders.AddLast(_order);
        SendNewOrderListToUI(orders.Last.Value);
    }

    public LinkedList<Order> GetOrderList() { return orders; }

    public void SendNewOrderListToUI(Order _order)  //CookUI에 주문 정보 전달
    {
        bool preActive = cookUI.activeSelf;
        cookUI.SetActive(true);
        cookUI.GetComponent<CookingUI>().AppendNodeOrderUI(_order);
        cookUI.SetActive(preActive);
    }

}
