using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessPositionOfObject : MonoBehaviour {
    GameObject[] accessGrounds;

    public void InitAccessGround()
    {
        int i;
        accessGrounds = new GameObject[4];
        RaycastHit2D[] hit;

        //상하좌우 각 방향에 존재하는 Ground오브젝트를 탐색하여 리스트에 추가
        //왼쪽
        hit = Physics2D.RaycastAll((Vector2)transform.parent.position + Vector2.left, Vector2.zero);
        for (i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null && hit[i].collider.gameObject.CompareTag("Ground")) { accessGrounds[0] = hit[i].collider.gameObject; }
        }

        //오른쪽
        hit = Physics2D.RaycastAll((Vector2)transform.parent.position + Vector2.right, Vector2.zero);
        for (i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null && hit[i].collider.gameObject.CompareTag("Ground")) { accessGrounds[1] = hit[i].collider.gameObject; }
        }

        //아래쪽
        hit = Physics2D.RaycastAll((Vector2)transform.parent.position + Vector2.down, Vector2.zero);
        for (i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null && hit[i].collider.gameObject.CompareTag("Ground")) { accessGrounds[2] = hit[i].collider.gameObject; }
        }

        //위쪽
        hit = Physics2D.RaycastAll((Vector2)transform.parent.position + Vector2.up, Vector2.zero);
        for (i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null && hit[i].collider.gameObject.CompareTag("Ground")) { accessGrounds[3] = hit[i].collider.gameObject; }
        }

    }


    public GameObject GetLeftGround() { return accessGrounds[0]; }
    public GameObject GetRightGround() { return accessGrounds[1]; }
    public GameObject GetDownGround() { return accessGrounds[2]; }
    public GameObject GetUpGround() { return accessGrounds[3]; }
    public GameObject[] GetAllGround() { return accessGrounds; }

}
