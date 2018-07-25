using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayAtCounter : MonoBehaviour {
    private Vector2 castPosition;
    private GameObject targetPlayer;
	
	void Start () {
        castPosition = transform.GetChild(0).position;
        targetPlayer = null;
	}
	
	
	void Update () {

        if (GetComponent<ObjectEventManager>().doEvent)
        {   //이벤트 매니저 플래그 발생
            targetPlayer = GetComponent<ObjectEventManager>().currentPlayer;
            GetComponent<ObjectEventManager>().doEvent = false;
            if (targetPlayer != null)
            {   //castPosition에 있는 땅으로 이동시킴
                targetPlayer.GetComponent<MyCharacterMove>().MoveThisGround(GetCounterGround(castPosition));
            }
        }

	}

    private GameObject GetCounterGround(Vector2 pos)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(pos, Vector2.zero);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Ground")) { return hit[i].collider.gameObject; }
        }

        return null;
    }
}
