using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayAtCounter : MonoBehaviour
{

    //child(0) : Counter Position For Cast
    //child(1) : Pay Position For Guest

    private GameObject targetPlayer;
    private Queue<GameObject> guestQueue;

    void Start()
    {
        guestQueue = new Queue<GameObject>();
        targetPlayer = null;
    }


    void Update()
    {

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저 플래그 발생
            targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            GetComponent<ObjectEventManager>().doEvent = false;
            if (targetPlayer != null)
            {   //castPosition에 있는 땅으로 이동시킴
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(GetCounterGroundForCast());
            }
        }

    }

    private GameObject GetPayGroundForGuest()
    {
        Vector2 guestPosition = transform.GetChild(1).position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(guestPosition, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    private GameObject GetCounterGroundForCast()
    {
        Vector2 castPosition = transform.GetChild(0).position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(castPosition, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    private GameObject GetGround(Vector2 _pos)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(_pos, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }

    public GameObject EnqueueGuestForPay(GameObject _guest)
    {
        guestQueue.Enqueue(_guest);
        Vector2 pos = (Vector2)transform.GetChild(1).position + Vector2.down * (guestQueue.Count - 1);
        return GetGround(pos);
    }

    public GameObject PeekGuestFromQueue() { return guestQueue.Peek(); }

}
