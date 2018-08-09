using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STATE = EnumCollection.G_STATE;

public class GuestAnimation : MonoBehaviour {
    public Sprite frontSprite;
    public Sprite sideSprite;   //Left Default
    public Sprite backSprite;
    public Sprite sit;

    private STATE curState;
    private Vector2 direct;
    private Vector2 prePos;
    

    void Start () {
        prePos = transform.position;
	}
	

	void Update ()
    {
        curState = GetComponentInParent<GuestAction>().currentState;
        switch (curState) {
            case STATE.MOVE:
                MoveAnimation();
                break;
            case STATE.HOLD:
                StopAnimation();
                break;
        }
    }

    void MoveAnimation()
    {
        if (prePos != (Vector2)transform.position)
        {
            if (Mathf.Abs(prePos.x - transform.position.x) >= Mathf.Abs(prePos.y - transform.position.y))
            {
                //오른쪽 이동
                if (prePos.x < transform.position.x) { direct = Vector2.right; }
                //왼쪽 이동
                else { direct = Vector2.left; }
            }
            else
            {
                //위로 이동
                if (prePos.y < transform.position.y) { direct = Vector2.up; }
                //아래 이동
                else { direct = Vector2.down; }
            }

            prePos = transform.position;
        }
        if (direct == Vector2.left) {
            GetComponent<SpriteRenderer>().sprite = sideSprite;
            transform.localScale = Vector2.one;
        }
        else if (direct == Vector2.right)
        {
            GetComponent<SpriteRenderer>().sprite = sideSprite;
            transform.localScale = Vector2.left + Vector2.up;
        }
        else if (direct == Vector2.up)
        {
            GetComponent<SpriteRenderer>().sprite = backSprite;
            transform.localScale = Vector2.one;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = frontSprite;
            transform.localScale = Vector2.one;
        }
    }

    void StopAnimation()
    {
        GameObject myTable = GetComponentInParent<GuestAction>().GetMyTable();
        if (myTable == null) { return; }
        Vector2 tablePos = myTable.transform.position;

        //테이블 왼쪽에 앉아있으면
        if (transform.position.x < tablePos.x)
        {
            direct = Vector2.right;
            MoveAnimation();
        }
        //테이블 오른쪽에 앉아있으면
        else if (transform.position.x > tablePos.x)
        {
            direct = Vector2.left;
            MoveAnimation();
        }
        //테이블 아래에 앉아있으면 -> 사실상 버그 외엔 존재하지 않는 경우
        else
        {
            direct = Vector2.up;
            MoveAnimation();
        }
    }

}
