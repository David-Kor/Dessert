using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInfoList : MonoBehaviour
{
    public GameObject cookUI;
    private LinkedList<Order> orders;
    private int preCount;
    private int idCounter;
    private Dictionary<Order, int> completeCounter; //<주문, 완성된 메뉴의 개수> 쌍의 딕셔너리
    
    void Start()
    {
        completeCounter = new Dictionary<Order, int>();
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

    void RemoveOrder(Order _order)
    {
        if (orders.Contains(_order))
        {
            orders.Remove(_order);
            return;
        }
        LinkedListNode<Order> node = orders.First;
        while (node != null)
        {
            if (node.Value.table == _order.table)
            {
                orders.Remove(node);
                return;
            }
            node = node.Next;
        }

    }

    public void AppendOrder(List<int> _idList, GameObject _table)   //직접 주문 정보를 받아 새 Order추가
    {
        orders.AddLast(new Order
        {
            orderID = idCounter++,
            table = _table,
            menuIDList = _idList,
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
        cookUI.GetComponent<CookingUI>().AppendOrderUI(_order);
        cookUI.SetActive(preActive);
    }

    public void CheckCompleteOrder(Order _order, MenuData.Menu _menu)
    {
        //처음 메뉴가 완성된 주문이면 딕셔너리 추가
        if (!completeCounter.ContainsKey(_order))
        {
            completeCounter.Add(_order, 0);
        }
        completeCounter[_order] = completeCounter[_order] + 1;  //Value를 1 더함 -> 해당 주문에 대한 완성된 메뉴의 수 +1

        if (completeCounter[_order] == _order.menuIDList.Count) //이 주문의 모든 메뉴가 완성 되었으면
        {
            GameObject.Find("CompletedOrders").GetComponent<CompletedOrders>().AddCompleteMenu(_order);
            RemoveOrder(_order);
        }
    }

}
