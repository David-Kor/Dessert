using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInfoList : MonoBehaviour
{
    public LinkedList<Order> orders;
    private int preCount;

    void Start()
    {
        orders = new LinkedList<Order>();
        preCount = 0;
    }
    void Update()
    {
        if (preCount != orders.Count)
        {
            TransmissionOrderListToUI();
            preCount = orders.Count;
        }
    }

    public void AppendOrder(List<int> _idList, GameObject _table)   //직접 주문 정보를 받아 새 Order추가
    {
        orders.AddLast(new Order
        {
            table = _table,
            menuIDList = _idList,
            enabled = false
        });
    }

    public void AppendOrder(Order _order)   //Order클래스로 받아 새 Order추가
    { orders.AddLast(_order); }

    public void TransmissionOrderListToUI()
    {
        LinkedListNode<Order> node = orders.First;
        while (node != null)
        {

            for (int i = 0; i < node.Value.menuIDList.Count; i++)
            {
                Debug.Log("[" + node.Value.table.GetComponent<TableStateManager>().tableID +
                    "] : " + MenuData.ConvertMenuIDToName(node.Value.menuIDList[i]));
            }
            if (!node.Value.enabled) { node.Value.enabled = true; }
            node = node.Next;
        }
    }

}
