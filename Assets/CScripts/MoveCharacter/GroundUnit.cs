using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUnit : MonoBehaviour {

    public bool isPassable;
    private int[] myIndex = new int[2];   //[0]은 수평위치(인덱스), [1]은 수직위치(인덱스) => 논리적(x, y)
    private List<string> unpassableObjectTagList;   //지나갈 수 없게 하는 오브젝트 리스트
    private bool absolutePassable;    //이 변수가 false면 무조건 지나갈 수 없게 됨

	void Start () {
        isPassable = true;
        unpassableObjectTagList = new List<string>
        {
            "Object",
            "Wall"
        };
        absolutePassable = true;
    }


    void Update()
    {
        if (absolutePassable)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.zero);
            //RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, Vector2.one*0.9f, 0f, Vector2.zero);

            //충돌체가 감지 되면 모든 오브젝트의 태그 검사
            if (hit.Length > 0)
            {

                int i;

                if (isPassable)
                {
                    for (i = 0; i < hit.Length; i++)
                    {
                        //지나갈 수 있는 상태에서 이 땅 위에 오브젝트가 올라오면 지나갈수 없는 상태로 변경
                        if (unpassableObjectTagList.IndexOf(hit[i].collider.gameObject.tag) >= 0) { isPassable = false; }
                    }
                }
                else
                {
                    bool isChange = true;
                    for (i = 0; i < hit.Length; i++)
                    {
                        //지나갈 수 없는 상태에서 이 땅 위에 오브젝트가 올라와있지 않으면 지나갈 수 있는 상태로 변경
                        if (unpassableObjectTagList.IndexOf(hit[i].collider.gameObject.tag) >= 0) { isChange = false; }
                    }
                    if (isChange) { isPassable = true; }
                }

            }

        }
        else
        {
            isPassable = false;
        }


    }

    public void setIndex(int _i, int _j)
    {
        myIndex[0] = _i;
        myIndex[1] = _j;
    }

    public int[] getIndex() { return myIndex; }

    public bool getIsPassable() { return isPassable; }

    public void SetAbsolutePassable(bool _passable) { absolutePassable = _passable; }

}
