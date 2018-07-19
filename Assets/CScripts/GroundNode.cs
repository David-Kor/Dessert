using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNode : MonoBehaviour {
    private int gScore;   //시작점으로부터의 거리
    private int dist;    //목적지까지의 추측거리
    private int[] myIndex;
    private Vector2 thisPos;
    private GroundNode parentNode;


    // Use this for initialization
    void Start()
    {
        gScore = 0;
        dist = 0;
        thisPos = transform.position;
        parentNode = null;
        myIndex = gameObject.GetComponent<GroundUnit>().getIndex();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public Vector2 GetPosition() { return thisPos; }

    public bool IsPassable() { return GetComponent<GroundUnit>().getIsPassable(); }

    public bool IsSamePosition(Vector2 _vector) { return _vector == thisPos; }

    public bool IsSamePosition(GroundNode _node) { return _node.GetPosition() == thisPos; }

    public void SetParent(GroundNode _parent) { parentNode = _parent; }

    public GroundNode GetParent() { return parentNode; }

    public int GetGScore() { return gScore; }

    public void SetGScore(int _gScore) { gScore = _gScore; }

    public void SetHScore(int _dist) { dist = _dist; }

    public int GetFScore() { return gScore + dist; }

    public int[] GetIndex() { return myIndex; }

    public int CalcDist(GroundNode _dest)
    {
        int x = Mathf.FloorToInt(Mathf.Abs(thisPos.x - _dest.GetPosition().x));
        int y = Mathf.FloorToInt(Mathf.Abs(thisPos.y - _dest.GetPosition().y));

        return x*10 + y*10;
    }


    public int CalcDistNeighbor(GroundNode _neighbor)
    {

        int x = Mathf.FloorToInt(Mathf.Abs(thisPos.x - _neighbor.GetPosition().x));
        int y = Mathf.FloorToInt(Mathf.Abs(thisPos.y - _neighbor.GetPosition().y));

        if (x + y == 2)
        {   //이웃노드가 대각선으로 인접한 경우
            return 14;
        }
        else if (x + y == 1)
        {   //이웃노드가 수직/수평선으로 인접한 경우
            return 10;
        }
        else
        {   //이웃노드가 비정상적(부모가 인접한 노드가 아니거나 자기자신인 경우)
            return 0;
        }

    }


    public int CalcGScore()
    {
        int myGScore;
        int x = Mathf.FloorToInt(Mathf.Abs(thisPos.x - parentNode.GetPosition().x));
        int y = Mathf.FloorToInt(Mathf.Abs(thisPos.y - parentNode.GetPosition().y));

        if (x + y == 2)
        {   //부모노드와 대각선으로 인접한 경우
            myGScore = 14;
        }
        else if (x + y == 1)
        {   //부모노드와 수직/수평으로 인접한 경우
            myGScore = 10;
        }
        else
        {
            myGScore = 0;  //부모노드가 비정상적(부모가 인접한 노드가 아니거나 자기자신인 경우)
        }

        return myGScore;

    }

}
