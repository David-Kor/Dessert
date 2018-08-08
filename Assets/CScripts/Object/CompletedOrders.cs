using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedOrders : MonoBehaviour {
    
    //Child(0~2) : Completed Objects

    public Queue<Order> completeQueue;  //완성된 주문들을 모은 대기 큐

    void Start()
    {
        completeQueue = new Queue<Order>();
    }

    public void AddCompleteMenu(Order _order)
    {
        completeQueue.Enqueue(_order);
        if (!transform.GetChild(2).GetComponent<SpriteRenderer>().enabled)   //가장 마지막 자식이 안보이게 되어있으면
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                //아직 안보이는 자식 오브젝트 하나를 보이도록 하고 Order정보를 넘김
                if (!transform.GetChild(i).GetComponent<SpriteRenderer>().enabled)
                {
                    //(이 경우 대기 큐에서 해당 Order를 Dequeque하므로 비어있게 됨)
                    transform.GetChild(i).GetComponent<CompletedObject>().SetOrder(completeQueue.Dequeue());
                    transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
                    return;
                }
            }
        }
    }

    public Order TakeCompleteMenu()
    {
        Order rtnOrder = null;

        //첫번째 자식이 활성화 되어있으면
        if (transform.GetChild(0).GetComponent<SpriteRenderer>().enabled)
        {
            //반환값을 첫번째 자식의 Order정보로 설정
            rtnOrder = transform.GetChild(0).GetComponent<CompletedObject>().GetOrder();

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                //자식들의 정보를 뒤에서 하나씩 당겨서 덮어씀
                if (transform.GetChild(i + 1).GetComponent<SpriteRenderer>().enabled)
                {
                    transform.GetChild(i).GetComponent<CompletedObject>().SetOrder(
                        transform.GetChild(i + 1).GetComponent<CompletedObject>().GetOrder()
                        );
                }
                else    //뒤의 자식이 비활성화 되어있으면 null값으로 대체하고 자신도 비활성화
                {
                    transform.GetChild(i).GetComponent<CompletedObject>().SetOrder(null);
                    transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            //대기 큐에 정보가 남아있으면 Dequeue해서 가져옴
            if (completeQueue.Count > 0)
            {
                transform.GetChild(transform.childCount - 1).GetComponent<CompletedObject>().SetOrder(completeQueue.Dequeue());
            }
            else
            {
                //큐가 비어있으면 마지막 자식 무조건 비활성화
                transform.GetChild(transform.childCount - 1).GetComponent<CompletedObject>().SetOrder(null);
                transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        return rtnOrder;
    }

}
