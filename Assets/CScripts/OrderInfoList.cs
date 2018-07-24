using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInfoList : MonoBehaviour
{
    public class Order
    {
        public GameObject table;
        public List<int> menuIDList;
    }

    LinkedList<Order> orders;

    void Start()
    {
        orders = new LinkedList<Order>();
    }

    public void AppendOrder(List<int> _idList, GameObject _table)
    {
        orders.AddLast(new Order
        {
            table = _table,
            menuIDList = _idList
        });
        Debug.Log(orders.Last.Value.table.GetComponent<TableStateManager>().tableID);
        for (int i = 0; i < orders.Last.Value.menuIDList.Count; i++)
        {
            Debug.Log("[" + (i + 1) + "] : " + orders.Last.Value.menuIDList[i] + MenuData.ConvertMenuIDToName(orders.Last.Value.menuIDList[i]));
        }
    }

}
